namespace EventManagement.Gateway.Middleware.NotFound
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class NotFoundMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger log;

        private readonly IConfigurationRoot configuration;

        public NotFoundMiddleware(RequestDelegate next, ILogger log, IConfigurationRoot configuration)
        {
            this.next = next;
            this.log = log;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            await this.next.Invoke(context);

            var statusCodeOnHeaderSubmit = context.Response.StatusCode;

            if (statusCodeOnHeaderSubmit == StatusCodes.Status404NotFound)
            {
                context.Response.Redirect("/NotFound");
            }
        }
    }
}
