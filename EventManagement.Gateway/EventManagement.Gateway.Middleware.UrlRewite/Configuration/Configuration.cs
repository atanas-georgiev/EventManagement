namespace EventManagement.Gateway.Middleware.UrlRewite.Configuration
{
    /// <summary>
    /// The configuration.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or sets the pattern.
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Gets or sets the replacement.
        /// </summary>
        public string Replacement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether set port.
        /// </summary>
        public bool SetPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether final.
        /// </summary>
        public bool Final { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether contains http.
        /// </summary>
        public bool ContainsHttp { get; set; }
    }
}
