namespace EventManagement.Gateway.Web.Proxy
{
    using EventManagement.Gateway.Middleware.Request;
    using EventManagement.Gateway.Middleware.UrlRewite;
    using EventManagement.Gateway.Shared;

    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// The proxy workflow.
    /// </summary>
    public static class ProxyWorkflow
    {
        /// <summary>
        /// The use step tunnel URL.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        public static void UseStepTunnelUrl(this IApplicationBuilder app, RequestOptions buffer)
        {
            app.UseMiddleware<TunnelUrlMiddleware>(buffer);
        }

        /// <summary>
        /// The use step request.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        public static void UseStepRequest(this IApplicationBuilder app, RequestOptions buffer)
        {
            app.UseMiddleware<RequestMiddleware>(buffer);
        }
    }
}
