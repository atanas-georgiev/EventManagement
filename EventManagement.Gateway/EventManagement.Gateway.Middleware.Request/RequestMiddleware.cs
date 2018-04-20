namespace EventManagement.Gateway.Middleware.Request
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    using EventManagement.Gateway.Shared;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// The request middleware.
    /// </summary>
    public class RequestMiddleware
    {

        private const int DefaultWebSocketBufferSize = 4096;

        /// <summary>
        /// The buffer.
        /// </summary>
        private readonly RequestOptions buffer;

        private readonly ILogger log;

        private static readonly string[] NotForwardedWebSocketHeaders =
            new[] { "Connection", "Host", "Upgrade", "Sec-WebSocket-Key", "Sec-WebSocket-Version", "User-Agent" };

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// The next.
        /// </param>
        public RequestMiddleware(RequestDelegate next, ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// The next.
        /// </param>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        public RequestMiddleware(RequestDelegate next, RequestOptions buffer, ILogger log)
        {
            this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            this.log = log;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                this.log.LogInformation(
                    $"RequestMiddleware > Invoke WebSocket {((Uri)context.Items[RequestOptions.TunnelUrl])}");
                await this.HandleWebSocketRequest(context);
            }
            else
            {
                this.log.LogInformation(
                    $"RequestMiddleware > Invoke Http {((Uri)context.Items[RequestOptions.TunnelUrl])}");
                await this.HandleHttpRequest(context);
            }
        }

        private async Task HandleWebSocketRequest(HttpContext context)
        {
            using (var client = new ClientWebSocket())
            {
                foreach (var headerEntry in context.Request.Headers)
                {
                    if (!NotForwardedWebSocketHeaders.Contains(headerEntry.Key, StringComparer.OrdinalIgnoreCase))
                    {
                        client.Options.SetRequestHeader(headerEntry.Key, headerEntry.Value);
                    }
                }

                var uriString = ((Uri)context.Items[RequestOptions.TunnelUrl]).ToString().Replace("http://", "ws://")
                    .Replace("https://", "wss://");

                if (this.buffer.WebSocketKeepAliveInterval.HasValue)
                {
                    client.Options.KeepAliveInterval = this.buffer.WebSocketKeepAliveInterval.Value;
                }

                try
                {
                    await client.ConnectAsync(new Uri(uriString), context.RequestAborted);
                }
                catch (WebSocketException wex)
                {
                    context.Response.StatusCode = 400;
                    this.log.LogError($"RequestMiddleware > WebSocketException: {wex.Message}", wex);
                    return;
                }

                try
                {
                    using (var server = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        await Task.WhenAll(
                            this.PumpWebSocket(client, server, context.RequestAborted),
                            this.PumpWebSocket(server, client, context.RequestAborted));
                    }
                }
                catch (Exception eex)
                {
                    this.log.LogError($"RequestMiddleware > AcceptWebSocket Exception: {eex.Message}", eex);
                }
            }
        }

        private async Task PumpWebSocket(WebSocket source, WebSocket destination, CancellationToken cancellationToken)
        {
            var buffer = new byte[this.buffer.WebSocketBufferSize ?? DefaultWebSocketBufferSize];
            while (true)
            {
                WebSocketReceiveResult result;
                try
                {
                    result = await source.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    await destination.CloseOutputAsync(
                        WebSocketCloseStatus.EndpointUnavailable,
                        null,
                        cancellationToken);
                    return;
                }
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await destination.CloseOutputAsync(
                        source.CloseStatus.Value,
                        source.CloseStatusDescription,
                        cancellationToken);
                    return;
                }

                await destination.SendAsync(
                    new ArraySegment<byte>(buffer, 0, result.Count),
                    result.MessageType,
                    result.EndOfMessage,
                    cancellationToken);
            }
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
        public async Task HandleHttpRequest(HttpContext context)
        {
            var httpClient = new HttpClient(new HttpClientHandler() { AllowAutoRedirect = false });
            var requestMessage = new HttpRequestMessage();
            var requestMethod = context.Request.Method;
            if (!HttpMethods.IsGet(requestMethod) && !HttpMethods.IsHead(requestMethod)
                && !HttpMethods.IsDelete(requestMethod) && !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(context.Request.Body);
                requestMessage.Content = streamContent;
            }
            foreach (var header in context.Request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            if (!requestMessage.Headers.Any(x => x.Key == this.buffer.Configuration["HeaderName"]))
            {
                requestMessage.Headers.Add(
                    this.buffer.Configuration["HeaderName"],
                    @"Bearer " + (string)context.Items[RequestOptions.AccessToken]);
            }

            requestMessage.Headers.Host = ((Uri)context.Items[RequestOptions.TunnelUrl]).Host + ":"
                                          + ((Uri)context.Items[RequestOptions.TunnelUrl]).Port;
            requestMessage.RequestUri = ((Uri)context.Items[RequestOptions.TunnelUrl]);
            requestMessage.Method = new HttpMethod(context.Request.Method);
            using (var responseMessage = await httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
            {
                var statusCode = (int)responseMessage.StatusCode;

                context.Response.StatusCode = statusCode;
                foreach (var header in responseMessage.Headers)
                {
                    context.Response.Headers[header.Key] = new StringValues(header.Value.ToArray());
                }

                foreach (var header in responseMessage.Content.Headers)
                {
                    context.Response.Headers[header.Key] = header.Value.FirstOrDefault();
                }

                context.Response.Headers.Remove("transfer-encoding");

                // Check if there is a redirect - could be with status code 3xx or 201
                if (responseMessage.Headers.Location != null)
                {
                    var location = context.Items["ServiceOriginPattern"] + responseMessage.Headers.Location.ToString();
                    context.Response.Headers["Location"] = new StringValues(new string[] { location });
                }

                if (statusCode != StatusCodes.Status401Unauthorized && statusCode != StatusCodes.Status403Forbidden)
                {
                    await responseMessage.Content.CopyToAsync(context.Response.Body);
                }
            }
        }
    }
}