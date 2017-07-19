using CookieIoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Web.Realtime;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Web
{
    public class WebSocketHandler : DependencyObject
    {
        private static CookieJar realtimeCookieJar;

        private WebSocket _ws;

        private RealtimeContext rtContext;

        public WebSocketHandler(ISContext serverContext, WebSocket websocket) : base(serverContext)
        {
            _ws = websocket;
            rtContext = new RealtimeContext(serverContext);
        }

        private async Task<string> ReadLineAsync()
        {
            var data = string.Empty;
            while (true)
            {
                var buf = new byte[1];
                var arraySeg = new ArraySegment<byte>(buf);
                await _ws.ReceiveAsync(arraySeg, CancellationToken.None);
                var c = (char)buf[0];
                data += c;
                if (c == '\n') return data;
            }
        }

        private async Task WriteLineAsync(string data)
        {
            await _ws.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        public async Task EventLoopAsync()
        {
            async Task<(JToken, string, long)> HandleRequestAsync(JObject requestBundle)
            {
                // Parse request
                var rcommand = ((JValue)requestBundle["request"]).ToObject<string>();
                var data = (JObject)requestBundle["data"];
                var id = ((JValue)requestBundle["id"]).ToObject<long>();
                // Get handler
                var handler = realtimeCookieJar.ResolveAll<IRealtimeHandler>().FirstOrDefault(x => x.Path == rcommand);
                return (await handler?.HandleRequestAsync(id, data, rtContext), rcommand, id);
            }
            var pipelineRegistered = false;
            var currentUser = default(RegisteredUser);
            var pipelineHandler = new Func<JObject, Task<bool>>(async (bundle) =>
            {
                await WriteLineAsync(bundle.ToString(Formatting.None));
                return false;
            });
            try
            {
                // Require auth first
                var authApiKey = await ReadLineAsync();
                // Attempt to authenticate
                if (!rtContext.AuthenticateWith(authApiKey))
                {
                    await WriteLineAsync("false"); // authentication failure
                    await _ws.CloseAsync(WebSocketCloseStatus.ProtocolError, "Invalid Authentication Key", CancellationToken.None);
                    throw new SecurityException();
                }
                await WriteLineAsync("true"); // websocket channel was authenticated successfully
                currentUser = await rtContext.GetCurrentUserAsync();
                // Attempt to add handler
                ServerContext.NotificationPipeline
                    .RetrieveUserPipeline(currentUser.Identifier)
                    .AddItemToEnd(pipelineHandler);
                pipelineRegistered = true;
                // pipeline is registered, send any queued, previously undelivered data
                var userNotificationQueue = new PlayerPersistentDataService(ServerContext).RetrieveNotificationQueue(currentUser.Identifier);
                while (userNotificationQueue.Count > 0)
                {
                    // send data back through pipelines
                    // now that a pipeline is registered, the data will be sent through the channel
                    // in the case of an exception or other delivery failure, the message will be queued again.
                    await ServerContext.NotificationPipeline.PushMessageAsync(userNotificationQueue.Dequeue(), currentUser.Identifier);
                }
                while (_ws.State == WebSocketState.Open)
                {
                    var rawData = await ReadLineAsync();
                    var requestBundle = JObject.Parse(rawData);
                    try
                    {
                        await HandleRequestAsync(requestBundle)
                            .ContinueWith(async t =>
                            {
                                (var response, var request, var id) = t.Result;
                                // Send result
                                var resultBundle = new JObject(
                                    new JProperty("id", id),
                                    new JProperty("data", response),
                                    new JProperty("type", "response"),
                                    new JProperty("request", request)
                                );
                                // Write result to websocket
                                await WriteLineAsync(resultBundle.ToString(Formatting.None));
                            });
                    }
                    catch (NullReferenceException) // Missing parameter
                    {
                        continue;
                    }
                }
            }
            finally
            {
                if (pipelineRegistered)
                {
                    // Unregister pipeline
                    ServerContext.NotificationPipeline
                        .RetrieveUserPipeline(currentUser.Identifier)
                        .UnregisterHandler(pipelineHandler);
                }
            }
        }

        public static async Task AcceptWebSocketClientsAsync(HttpContext hc, Func<Task> n, ISContext sctx)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var ws = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new WebSocketHandler(sctx, ws);
            await h.EventLoopAsync();
        }

        public static void Map(IApplicationBuilder app, ISContext context)
        {
            // DI for websocket handlers
            realtimeCookieJar = new RealtimeBootstrapper()
                .ConfigureRealtimeHandlerContainer(context);
            app.Use(async (hc, n) => await WebSocketHandler.AcceptWebSocketClientsAsync(hc, n, context));
        }
    }
}
