namespace EventManagement.Gateway.Middleware.UrlRewite
{
    using System;
    using System.Threading.Tasks;

    using EventManagement.Gateway.Middleware.UrlRewite.Configuration;
    using EventManagement.Gateway.Middleware.UrlRewite.Helper;
    using EventManagement.Gateway.Shared;

    using Microsoft.ApplicationInsights.AspNetCore.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The tunnel url middleware.
    /// </summary>
    public class TunnelUrlMiddleware
    {
        /// <summary>
        /// The buffer.
        /// </summary>
        private readonly RequestOptions buffer;

        /// <summary>
        /// The next
        /// </summary>
        private readonly RequestDelegate next;

        private readonly Configurations configurations;

        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TunnelUrlMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// The next.
        /// </param>
        /// <param name="log">
        /// The log
        /// </param>
        public TunnelUrlMiddleware(RequestDelegate next, ILogger log)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.log = log;
            this.configurations = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TunnelUrlMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// The next.
        /// </param>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        /// <param name="log">
        /// The log
        /// </param>
        public TunnelUrlMiddleware(RequestDelegate next, RequestOptions buffer, ILogger log)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            this.log = log;
            this.configurations = new Configurations(this.buffer.Configuration);
        }

        /// <summary>
        /// The invoke.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task Invoke(HttpContext context)
        {
            var uri = context.Request.GetUri();
            var bufferUri = uri.ToString();
            var urlConfiguration = new Configuration.Configuration();

            if (uri.PathAndQuery.Length <= 1)
            {
                uri = uri.SetPath(this.configurations.DefaultRedirectRoute);
            }

            var final = false;
            while (!final)
            {
                uri = this.UrlRewrite(uri, out urlConfiguration);
                final = urlConfiguration.Final;
                if (urlConfiguration.SetPort)
                {
                    uri = uri.SetPort(Convert.ToInt32(this.configurations.ProxyPort));
                }
            }

            context.Items[RequestOptions.TunnelUrl] = uri;
            context.Items[RequestOptions.ServiceOriginPattern] = urlConfiguration.Pattern;

            this.log.LogDebug($"TunnelUrlMiddleware: {bufferUri} - {uri}");
            await this.next.Invoke(context);
        }

        private Uri UrlRewrite(Uri original, out Configuration.Configuration urlConfiguration)
        {
            urlConfiguration = new Configuration.Configuration();
            urlConfiguration.SetPort = true;
            urlConfiguration.Final = true;
            foreach (var rewtireSet in this.configurations.Items)
            {
                if (original.AbsoluteUri.Contains(rewtireSet.Pattern))
                {
                    var newUri = original.AbsoluteUri.Replace(rewtireSet.Pattern, rewtireSet.Replacement);
                    if (rewtireSet.ContainsHttp)
                    {
                        newUri = newUri.Substring(newUri.IndexOf(rewtireSet.Replacement, StringComparison.Ordinal));
                    }

                    original = new Uri(newUri);
                    urlConfiguration = rewtireSet;
                    break;
                }
            }

            return original;
        }
    }
}
