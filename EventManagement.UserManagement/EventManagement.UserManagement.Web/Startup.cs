namespace EventManagement.UserManagement.Web
{
    using System;
    using System.Text;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;

    using EventManagement.UserManagement.Data;
    using EventManagement.UserManagement.Data.Seed;
    using EventManagement.UserManagement.Shared;
    using EventManagement.UserManagement.Shared.Extensions;
    using EventManagement.UserManagement.Shared.Models;
    using EventManagement.UserManagement.Web.Services;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
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
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceScopeFactory scopeFactory, RoleManager<IdentityRole> roleManager)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            scopeFactory.SeedData(roleManager);

            app.UseAuthentication();

            //app.UseWhen(
            //    context => context.Request.Path.ToString().StartsWith("/Account"),
            //    appBuilder => { app.UseAuthentication(); });

            //app.UseWhen(
            //    context => !context.Request.Path.ToString().StartsWith("/Account"),
            //    appBuilder => { app.UseEventManagementBearerAuthentication(); });

            app.UseStaticFiles();
            app.UseMvc();
        }
        private IContainer applicationContainer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserManagementDbContext>(
                options =>
                    {
                        options.UseSqlServer(this.Configuration.GetConnectionString("UserManagementDbContext"));
                    });

            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UserManagementDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IConfigurationRoot>(res => this.Configuration);

            services.AddEventManagementAuthentication(new DefaultEventManagementTokenValidationParameters(this.Configuration));

            services.AddMvc().AddRazorPagesOptions(o =>
                {
                    o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                });
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            //services.AddMvc().AddRazorPagesOptions(
            //    options =>
            //        {
            //            options.Conventions.AuthorizeFolder("/EventManagement/UserManagement/Account/Manage");
            //            options.Conventions.AuthorizePage("/EventManagement/UserManagement/Account/Logout");
            //        });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            // builder.Register(r => this.Configuration).As<IConfigurationRoot>().SingleInstance();
            this.applicationContainer = builder.Build();
            return new AutofacServiceProvider(this.applicationContainer);
        }
    }
}