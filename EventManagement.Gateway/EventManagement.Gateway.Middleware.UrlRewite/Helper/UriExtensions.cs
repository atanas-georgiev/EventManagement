namespace EventManagement.Gateway.Middleware.UrlRewite.Helper
{
    using System;

    /// <summary>
    /// The uri extensions.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// The set port.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="newPort">
        /// The new port.
        /// </param>
        /// <returns>
        /// The <see cref="Uri"/>.
        /// </returns>
        public static Uri SetPort(this Uri uri, int newPort)
        {
            var builder = new UriBuilder(uri) { Port = newPort };
            return builder.Uri;
        }

        /// <summary>
        /// The set host.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="host">The new host</param>
        /// <returns>The <see cref="Uri"/></returns>
        public static Uri SetHost(this Uri uri, string host)
        {
            var builder = new UriBuilder(uri) { Host = host };
            return builder.Uri;
        }

        /// <summary>
        /// Sets http to uri
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns>The <see cref="Uri"/></returns>
        public static Uri SetHttp(this Uri uri)
        {
            var builder = new UriBuilder(uri) { Scheme = "http" };
            return builder.Uri;
        }

        /// <summary>
        /// Sets path to uri
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="path">The new path</param>
        /// <returns>The <see cref="Uri"/></returns>
        public static Uri SetPath(this Uri uri, string path)
        {
            var builder = new UriBuilder(uri) { Path = path };
            return builder.Uri;
        }
    }
}
