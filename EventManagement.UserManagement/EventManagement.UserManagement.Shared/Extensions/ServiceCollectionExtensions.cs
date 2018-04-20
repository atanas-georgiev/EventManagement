namespace EventManagement.UserManagement.Shared.Extensions
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    using JwtAuthenticationHelper.Abstractions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.WindowsAzure.Storage;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventManagementAuthentication(
            this IServiceCollection services,
            TokenValidationParameters tokenValidationParams,
            string applicationDiscriminator = null,
            AuthUrlOptions authUrlOptions = null)
        {
            if (tokenValidationParams == null)
            {
                throw new ArgumentNullException(
                    $"{nameof(tokenValidationParams)} is a required parameter. "
                    + $"Please make sure you've provided a valid instance with the appropriate values configured.");
            }

            var configuration = services.BuildServiceProvider().GetService<IConfigurationRoot>();
            var env = services.BuildServiceProvider().GetService<IHostingEnvironment>();

            if (env.IsDevelopment() || env.IsEnvironment("IntegrationTest"))
            {
                var userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var keysFolder = Path.Combine(userFolder, "EventManagementKeys");

                if (!Directory.Exists(keysFolder))
                {
                    Directory.CreateDirectory(keysFolder);
                }

                services
                    .AddDataProtection(options => options.ApplicationDiscriminator = configuration["Security:ApplicationDiscriminator"])
                    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder));
            }
            else
            {
                var storageAccount = CloudStorageAccount.Parse(configuration["Security:AzureKeysBlob"]);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference("xml-keys");
                container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

                services
                    .AddDataProtection(options => options.ApplicationDiscriminator = configuration["Security:ApplicationDiscriminator"])
                    .PersistKeysToAzureBlobStorage(container, "keys");  
            }

            services.AddScoped<IDataSerializer<AuthenticationTicket>, TicketSerializer>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(
                serviceProvider
                    => new JwtTokenGenerator(tokenValidationParams.ToTokenOptions(int.Parse(configuration["Security:TokenAndCookieExparationTimeMinutes"]))));

            services.AddAuthentication(
                options =>
                    {
                        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    }).AddCookie(
                options =>
                    {
                        options.Cookie.Expiration = TimeSpan.FromMinutes(int.Parse(configuration["Security:TokenAndCookieExparationTimeMinutes"]));
                        options.TicketDataFormat = new JwtAuthTicketFormat(
                                                    tokenValidationParams,
                                                    services.BuildServiceProvider().GetService<IDataSerializer<AuthenticationTicket>>(),
                                                    services.BuildServiceProvider().GetDataProtector(new[] { $"{configuration["Security:ApplicationDiscriminator"]}-Auth" }));

                        options.Events = new CookieAuthenticationEvents { OnRedirectToLogin = OnRedirectToLogin };
                    });


            return services;
        }

        private static Task OnRedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            // override the status code
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

    }

    public sealed class AuthUrlOptions
    {
        public string LoginPath { get; set; }

        public string LogoutPath { get; set; }

        public string ReturnUrlParameter { get; set; }
    }
}