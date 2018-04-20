using System;

namespace EventManagement.Gateway.Shared
{
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;

    public class RequestOptions
    {
        /// <summary>
        /// The test token.
        /// </summary>
        public const string TestToken = "TestToken";

        /// <summary>
        /// The SAML token.
        /// </summary>
        public const string SamlToken = "SamlToken";

        /// <summary>
        /// The access token.
        /// </summary>
        public const string AccessToken = "AccessToken";

        /// <summary>
        /// The tunnel url.
        /// </summary>
        public const string TunnelUrl = "TunnelUrl";

        /// <summary>
        /// Pattern of the originating service
        /// </summary>
        public const string ServiceOriginPattern = "ServiceOriginPattern";

        /// <summary>
        /// The web socket buffer size.
        /// </summary>
        private int? webSocketBufferSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestOptions"/> class.
        /// </summary>
        /// <param name="memoryCache">
        /// The memory cache.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public RequestOptions(/*IMemoryCache memoryCache, */IConfigurationRoot configuration)
        {
            //this.Cache = memoryCache;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        //public IMemoryCache Cache { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        public IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Gets or sets the web socket keep alive interval.
        /// </summary>
        public TimeSpan? WebSocketKeepAliveInterval { get; set; }

        /// <summary>
        /// Gets or sets the web socket buffer size.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Bad buffer size
        /// </exception>
        public int? WebSocketBufferSize
        {
            get => this.webSocketBufferSize;
            set
            {
                if (value.HasValue && value.Value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                this.webSocketBufferSize = value;
            }
        }
    }
}
