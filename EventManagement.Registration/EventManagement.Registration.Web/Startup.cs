namespace EventManagement.Registration.Web
{
    using System;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using EventManagement.Payment.Shared;
    using EventManagement.Payment.Shared.Events;
    using EventManagement.Registration.Data;
    using EventManagement.Registration.Data.Seed;
    using EventManagement.Registration.Services;
    using EventManagement.Registration.Shared;
    using EventManagement.Resources.Shared;
    using EventManagement.Resources.Shared.Events;
    using EventManagement.Shared.ServiceBus;
    using EventManagement.UserManagement.Shared;
    using EventManagement.UserManagement.Shared.Extensions;
    using EventManagement.UserManagement.Shared.Testing.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using NServiceBus;
    using NServiceBus.Testing;

    public class Startup
    {
        private IContainer applicationContainer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder;

            if (env.IsDevelopment())
            {
                builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.Development.json", optional: true).AddEnvironmentVariables();
            }
            else
            {
                builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
            }

            this.Configuration = builder.Build();
            this.CurrentEnvironment = env;
        }

        public static IMessageSession MessageSession { get; set; }

        public IConfigurationRoot Configuration { get; }

        private IHostingEnvironment CurrentEnvironment { get; }

        public static IServiceProvider ServiceProvider { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceScopeFactory scopeFactory)
        {
            app.UseAuthentication();
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            if (this.CurrentEnvironment.EnvironmentName == "IntegrationTest")
            {
                app.UseTesting();
            }
            else
            {
                scopeFactory.SeedData();
            }

            app.UseMvc(
                routes =>
                    {
                        routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");

                        routes.MapSpaFallbackRoute(
                            name: "spa-fallback",
                            defaults: new { controller = "Home", action = "Index" });
                    });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceFabricHostMarker = Environment.GetEnvironmentVariable("ServiceFabricHostMarker");

            if (this.CurrentEnvironment.EnvironmentName == "IntegrationTest")
            {
                var context = this.CreateInMemoryDbContextBuilder();
                services.AddSingleton(context);
                IntegrationSeed.Execute(context);
            }
            else
            {
                services.AddDbContext<RegistrationDbContext>(
                    options => options.UseSqlServer(this.Configuration.GetConnectionString("RegistrationDbContext")));
            }

            services.AddSingleton(res => this.Configuration);
            services.AddScoped<DbContext, RegistrationDbContext>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IRegistrationService, RegistrationService>();

            services.AddCors();
            services.AddMvc();
            services.AddEventManagementAuthentication(
                new DefaultEventManagementTokenValidationParameters(this.Configuration));

            var builder = new ContainerBuilder();
            builder.Populate(services);

            // Add NServiceBus Handlers
            EndpointConfiguration endpointConfiguration = null;
            if (!this.CurrentEnvironment.IsEnvironment("IntegrationTest"))
            {
                endpointConfiguration = EndpointConfigurationFactory.ConfigureEndpoint(RegistrationEndpoint.Name, true)
                    .ConfigureSqlPersistence(
                        this.Configuration.GetConnectionString("RegistrationDbContext"),
                        1,
                        this.CurrentEnvironment.IsDevelopment() && serviceFabricHostMarker == null).ConfigureSqlTransport(
                        this.Configuration.GetConnectionString("TransportDbContext"),
                        (routing) =>
                            {
                                routing.RegisterPublisher(typeof(AddEvent), ResouresEndpoint.Name);
                                routing.RegisterPublisher(typeof(DeleteEvent), ResouresEndpoint.Name);
                                routing.RegisterPublisher(typeof(CompletePayment), PaymentEndpoint.Name);
                                routing.RegisterPublisher(typeof(CancelPayment), PaymentEndpoint.Name);
                            });
                endpointConfiguration.LicensePath("License.xml");
            }

            builder.Register(x => MessageSession).As<IMessageSession>();

            // Build container
            this.applicationContainer = builder.Build();

            // Add NServiceBus
            if (!this.CurrentEnvironment.IsEnvironment("IntegrationTest"))
            {
                endpointConfiguration.UseContainer<AutofacBuilder>(
                    customizations => { customizations.ExistingLifetimeScope(this.applicationContainer); });
                MessageSession = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            }
            else
            {
                MessageSession = new TestableMessageSession();
            }

            var result = new AutofacServiceProvider(this.applicationContainer);
            ServiceProvider = result;

            return result;
        }

        private RegistrationDbContext CreateInMemoryDbContextBuilder()
        {
            var optionsBuilder = new DbContextOptionsBuilder<RegistrationDbContext>();
            optionsBuilder.UseInMemoryDatabase("RegistrationDbContext");

            return new RegistrationDbContext(optionsBuilder.Options);
        }
    }
}