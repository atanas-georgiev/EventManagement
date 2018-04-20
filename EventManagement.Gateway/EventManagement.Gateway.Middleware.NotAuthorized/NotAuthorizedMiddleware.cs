namespace EventManagement.Gateway.Middleware.NotAuthorized
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class NotAuthorizedMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger log;

        private readonly IConfigurationRoot configuration;

        public NotAuthorizedMiddleware(RequestDelegate next, ILogger log, IConfigurationRoot configuration)
        {
            this.next = next;
            this.log = log;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            await this.next.Invoke(context);

            var statusCodeOnHeaderSubmit = context.Response.StatusCode;

            if (statusCodeOnHeaderSubmit == StatusCodes.Status401Unauthorized || statusCodeOnHeaderSubmit == StatusCodes.Status403Forbidden)
            {
                context.Response.Redirect("/NotAuthorized");
            }
        }
    }
}
