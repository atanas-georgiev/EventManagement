namespace EventManagement.Gateway.Middleware.Error
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class ErrorMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger log;

        private readonly IConfigurationRoot configuration;

        public ErrorMiddleware(RequestDelegate next, ILogger log, IConfigurationRoot configuration)
        {
            this.next = next;
            this.log = log;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            await this.next.Invoke(context);

            var statusCodeOnHeaderSubmit = context.Response.StatusCode;

            if (statusCodeOnHeaderSubmit == StatusCodes.Status500InternalServerError)
            {
                context.Response.Redirect("/Error");
            }
        }
    }
}
