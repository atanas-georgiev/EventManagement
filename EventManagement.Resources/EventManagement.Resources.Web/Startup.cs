namespace EventManagement.Resources.Web
{
    using System;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using EventManagement.Resources.Data;
    using EventManagement.Resources.Data.Seed;
    using EventManagement.Resources.Services;
    using EventManagement.Resources.Shared;
    using EventManagement.Shared.ServiceBus;
    using EventManagement.UserManagement.Shared;
    using EventManagement.UserManagement.Shared.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using NServiceBus;

    using Swashbuckle.AspNetCore.Swagger;

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceScopeFactory scopeFactory)
        {
            app.UseAuthentication();

            app.UseDeveloperExceptionPage();

            //if (env.IsDevelopment())
            //{
            //    app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions { HotModuleReplacement = true });
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles();

            scopeFactory.SeedData();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contacts API V1");
                });

            app.UseMvc(
                routes =>
                    {
                        routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");

                        routes.MapSpaFallbackRoute(
                            name: "spa-fallback",
                            defaults: new { controller = "Home", action = "Index" });
                    });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var serviceFabricHostMarker = Environment.GetEnvironmentVariable("ServiceFabricHostMarker");

            services.AddDbContext<ResourcesDbContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("ResourcesDbContext")));

            services.AddSingleton(res => this.Configuration);
            services.AddScoped<DbContext, ResourcesDbContext>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IResourceService, ResourceService>();

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "Contacts API", Version = "v1" });
                });

            services.AddCors();
            services.AddMvc();
            services.AddEventManagementAuthentication(new DefaultEventManagementTokenValidationParameters(this.Configuration));

            var builder = new ContainerBuilder();
            builder.Populate(services);

            // Add NServiceBus Handlers
            EndpointConfiguration endpointConfiguration = null;
            if (!this.CurrentEnvironment.IsEnvironment("IntegrationTest"))
            {
                endpointConfiguration = EndpointConfigurationFactory.ConfigureEndpoint(ResouresEndpoint.Name, true)
                    .ConfigureSqlPersistence(
                        this.Configuration.GetConnectionString("ResourcesDbContext"),
                        1,
                        this.CurrentEnvironment.IsDevelopment() && serviceFabricHostMarker == null).ConfigureSqlTransport(
                        this.Configuration.GetConnectionString("TransportDbContext"),
                        (routing) =>
                            {
                                // TODO: Mapp app service bus nodes
                                // if (Convert.ToBoolean(this.Configuration.GetSection("ServiceBusNodes")["SomeNode"]))
                                // {
                                // routing.RegisterPublisher(typeof(SomeMessage), EndpointName);
                                // }
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
    }
}