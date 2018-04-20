namespace EventManagement.Payment.Web
{
    using System;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using EventManagement.Payment.Data;
    using EventManagement.Payment.Data.Seed;
    using EventManagement.Payment.Services;
    using EventManagement.Payment.Shared;
    using EventManagement.Payment.Shared.Events;
    using EventManagement.Registration.Shared;
    using EventManagement.Registration.Shared.Events;
    using EventManagement.Shared.ServiceBus;
    using EventManagement.UserManagement.Shared;
    using EventManagement.UserManagement.Shared.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using NServiceBus;

    public class Startup
    {
        private IContainer applicationContainer;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceFabricHostMarker = Environment.GetEnvironmentVariable("ServiceFabricHostMarker");

            services.AddDbContext<PaymentDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("PaymentDbContext")));

            services.AddSingleton(res => this.Configuration);
            services.AddScoped<DbContext, PaymentDbContext>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddEventManagementAuthentication(new DefaultEventManagementTokenValidationParameters(this.Configuration));
            services.AddMvc();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            // Add NServiceBus Handlers
            EndpointConfiguration endpointConfiguration = null;
            if (!this.CurrentEnvironment.IsEnvironment("IntegrationTest"))
            {
                endpointConfiguration = EndpointConfigurationFactory.ConfigureEndpoint(PaymentEndpoint.Name, true)
                    .ConfigureSqlPersistence(
                        this.Configuration.GetConnectionString("PaymentDbContext"),
                        1,
                        this.CurrentEnvironment.IsDevelopment() && serviceFabricHostMarker == null).ConfigureSqlTransport(
                        this.Configuration.GetConnectionString("TransportDbContext"),
                        (routing) =>
                            {
                                routing.RegisterPublisher(typeof(RequestPayment), RegistrationEndpoint.Name);
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

            return new AutofacServiceProvider(this.applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceScopeFactory scopeFactory)
        {
            app.UseAuthentication();
            app.UseDeveloperExceptionPage();

            scopeFactory.SeedData();

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
