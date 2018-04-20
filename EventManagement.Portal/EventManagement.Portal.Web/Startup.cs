namespace EventManagement.Portal.Web
{
    using System;

    using EventManagement.Portal.Business.AvailableSettings;
    using EventManagement.Portal.Business.UserSettings;
    using EventManagement.Portal.Data;
    using EventManagement.Portal.Data.Seed;
    using EventManagement.UserManagement.Shared;
    using EventManagement.UserManagement.Shared.Extensions;
    using EventManagement.UserManagement.Shared.Testing.Extensions;

    using KPMG.TaxRay.Portal.Business.Links;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
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
            
            this.logger = loggerFactory.CreateLogger("Portal");
            this.env = env;
        }

        private IConfigurationRoot configuration;

        private IHostingEnvironment env;

        private ILogger logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (this.env.EnvironmentName == "IntegrationTest")
            {
                var context = this.CreateInMemoryDbContextBuilder();
                services.AddSingleton(context);
                IntegrationSeed.Execute(context);
            }
            else
            {
                services.AddDbContext<PortalDbContext>(
                    options => options.UseSqlServer(this.configuration.GetConnectionString("PortalDbContext")));
            }
    
            services.AddScoped<DbContext, PortalDbContext>();
            services.AddScoped<ILinksService, LinksService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IAvailableSettingsService, AvailableSettingsService>();
            services.AddSingleton<IConfigurationRoot>(res => this.configuration);
            services.AddEventManagementAuthentication(new DefaultEventManagementTokenValidationParameters(this.configuration));
            services.AddCors();
            services.AddMvc();
        }

        private void OptionsAction(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceScopeFactory scopeFactory)
        {            
            app.UseAuthentication();

            if (this.env.EnvironmentName == "IntegrationTest")
            {
                app.UseTesting();
            }
            else
            {
                scopeFactory.SeedData();
            }

            if (this.env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseStaticFiles(new StaticFileOptions
                                   {
                                       OnPrepareResponse = context =>
                                           {
                                                context.Context.Response.Headers.Add("cache-control", new[] { "public,max-age=86400" });
                                                context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddDays(1).ToString("R") }); // Format RFC1123
                                           }
                                   });
            app.UseMvc();
        }

        private PortalDbContext CreateInMemoryDbContextBuilder()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PortalDbContext>();
            optionsBuilder.UseInMemoryDatabase("PortalDbContext");

            return new PortalDbContext(optionsBuilder.Options);
        }
    }
}
