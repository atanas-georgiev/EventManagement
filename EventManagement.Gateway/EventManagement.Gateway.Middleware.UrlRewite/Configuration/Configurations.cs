namespace EventManagement.Gateway.Middleware.UrlRewite.Configuration
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Configuration;

    public class Configurations
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configurations"/> class.
        /// </summary>
        /// <param name="root">
        /// The root.
        /// </param>
        public Configurations(IConfigurationRoot root)
        {
            this.Items = new List<Configuration>();
            var uriRewrites = root.GetSection("UrlRewrite");
            foreach (var rewtireSet in uriRewrites.GetChildren())
            {
                this.Items.Add(
                    new Configuration()
                        {
                            Pattern = rewtireSet["pattern"],
                            Replacement = rewtireSet["replacement"],
                            SetPort = rewtireSet["setPort"] == "True",
                            Final = rewtireSet["final"] == "True",
                            ContainsHttp =
                                rewtireSet["replacement"].Contains("http://")
                                || rewtireSet["replacement"].Contains("https://")
                        });
            }
            this.UseHttpOnly = root["UseHttpOnly"] == "True";
            this.UseLocalRedirectIp = root["UseLocalRedirectIp"] == "True";
            this.ProxyPort = root["ProxyPort"];
            this.DefaultRedirectRoute = root["DefaultRedirectRoute"];
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public List<Configuration> Items { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use http only.
        /// </summary>
        public bool UseHttpOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use local redirect IP.
        /// </summary>
        public bool UseLocalRedirectIp { get; set; }

        /// <summary>
        /// Gets or sets the proxy port.
        /// </summary>
        public string ProxyPort { get; set; }

        /// <summary>
        /// Gets or sets the default redirect route.
        /// </summary>
        public string DefaultRedirectRoute { get; set; }
    }
}
