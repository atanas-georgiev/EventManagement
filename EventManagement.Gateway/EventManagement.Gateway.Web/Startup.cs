namespace EventManagement.Gateway.Web
{
    using System;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using EventManagement.Gateway.Middleware.Error;
    using EventManagement.Gateway.Middleware.NotAuthorized;
    using EventManagement.Gateway.Middleware.NotFound;
    using EventManagement.Gateway.Shared;
    using EventManagement.Gateway.Web.Configuration;
    using EventManagement.Gateway.Web.Proxy;
    using EventManagement.UserManagement.Shared;
    using EventManagement.UserManagement.Shared.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            this.configuration = builder.Build();
            this.settings = new Settings(this.configuration);

            this.logger = loggerFactory.CreateLogger("EventManagement");
        }

        private readonly IConfigurationRoot configuration;

        private Settings settings;

        private readonly ILogger logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(
                options =>
                    {
                        options.Limits.MaxRequestLineSize = 65535;
                        options.Limits.MaxRequestHeadersTotalSize = 65535;
                    });

            services.AddLogging();
            services.AddMemoryCache();
            services.AddCors();
            services.AddMvc();

            services.AddSingleton<IConfigurationRoot>(res => this.configuration);
            services.AddEventManagementAuthentication(new DefaultEventManagementTokenValidationParameters(this.configuration));

            var builder = new ContainerBuilder();
            builder.Populate(services);

            // builder.Register(r => this.configuration).As<IConfigurationRoot>().SingleInstance();
            builder.Register(r => this.logger).As<ILogger>().SingleInstance();
            builder.RegisterType<RequestOptions>();

            // Build container
            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<NotAuthorizedMiddleware>();
            app.UseMiddleware<NotFoundMiddleware>();
            //app.UseMiddleware<ErrorMiddleware>();

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            app.UseAuthentication();

            app.UseCors(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            app.UseWebSockets();

            var buffer = app.ApplicationServices.GetService(typeof(RequestOptions)) as RequestOptions;
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseStepTunnelUrl(buffer);
            app.UseStepRequest(buffer);
        }
    }
}
