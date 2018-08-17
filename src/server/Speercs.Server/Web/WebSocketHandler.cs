using System;
using System.Linq;
using System.Net.WebSockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CookieIoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;
using Speercs.Server.Services.Game;
using Speercs.Server.Services.Metrics;
using Speercs.Server.Web.Realtime;

namespace Speercs.Server.Web {
    public class WebSocketHandler : DependencyObject {
        private static CookieJar _realtimeCookieJar;

        private WebSocket _ws;

        private RealtimeContext _rtContext;

        public WebSocketHandler(ISContext serverContext, WebSocket websocket) : base(serverContext) {
            _ws = websocket;
            _rtContext = new RealtimeContext(serverContext);
        }

        private async Task<string> readMessageAsync() {
            var message = new StringBuilder();
            while (true) {
                var buf = new byte[1024 * 16];
                var arraySeg = new ArraySegment<byte>(buf);
                var result = await _ws.ReceiveAsync(arraySeg, CancellationToken.None);
                var recvStr = System.Text.Encoding.UTF8.GetString(buf).Substring(0, result.Count);
                message.Append(recvStr);
                if (result.EndOfMessage) return message.ToString();
            }
        }

        private Task writeMessageAsync(string data) {
            lock (_ws) {
                return _ws.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(data + "\n")),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );
            }
        }

        private async Task<(JToken, string, long)> handleRequestAsync(JObject requestBundle) {
            // Parse request
            var rcommand = ((JValue) requestBundle["request"]).ToObject<string>();
            var data = (JObject) requestBundle["data"];
            var id = ((JValue) requestBundle["id"]).ToObject<long>();
            // Get handler
            var handler = _realtimeCookieJar.resolveAll<IRealtimeHandler>().FirstOrDefault(x => x.path == rcommand);
            var result = (JToken) JValue.CreateNull();
            if (handler != null) {
                result = await handler.handleRequestAsync(id, data, _rtContext);
            }

            return (result, rcommand, id);
        }

        public async Task eventLoopAsync() {
            var pipelineRegistered = false;
            var user = default(RegisteredUser);
            var pipelineHandler = new Func<JObject, Task<bool>>(async bundle => {
                await writeMessageAsync(bundle.ToString(Formatting.None));
                return false;
            });
            try {
                // Require auth first
                var authApiKey = await readMessageAsync();
                // Attempt to authenticate
                if (!_rtContext.authenticateWith(authApiKey)) {
                    await writeMessageAsync("false"); // authentication failure
                    await _ws.CloseAsync(WebSocketCloseStatus.ProtocolError, "Invalid authentication key",
                        CancellationToken.None);
                    throw new SecurityException();
                }

                await writeMessageAsync("true"); // websocket channel was authenticated successfully
                user = await _rtContext.getCurrentUserAsync();
                // Attempt to add handler
                serverContext.notificationPipeline
                    .retrieveUserPipeline(user.identifier)
                    .addEnd(pipelineHandler);
                pipelineRegistered = true;

                // save last connection metric
                var metricsService = new UserMetricsService(serverContext, user.identifier);
                var metricsObject = metricsService.get();
                metricsService.log(MetricsEventType.RealtimeConnect);
                metricsObject.lastRealtimeCollection =
                    (ulong) DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // pipeline is registered, send any queued, previously undelivered data
                var userNotificationQueue =
                    new PersistentDataService(serverContext).retrieveNotificationQueue(user.identifier);
                while (userNotificationQueue.Count > 0) {
                    // send data back through pipelines
                    // now that a pipeline is registered, the data will be sent through the channel
                    // in the case of an exception or other delivery failure, the message will be queued again.
                    await serverContext.notificationPipeline.pushMessageAsync(userNotificationQueue.Dequeue(),
                        "queue", user.identifier, true);
                }

                while (_ws.State == WebSocketState.Open) {
                    var rawData = await readMessageAsync();
                    var requestBundle = JObject.Parse(rawData);
                    try {
                        await handleRequestAsync(requestBundle)
                            .ContinueWith(async t => {
                                var (response, request, id) = t.Result;
                                // Send result
                                var resultBundle = new JObject(
                                    new JProperty("id", id),
                                    new JProperty("data", response),
                                    new JProperty("type", "response"),
                                    new JProperty("request", request)
                                );
                                // Write result to websocket
                                await writeMessageAsync(resultBundle.ToString(Formatting.None));
                            });
                    } catch (NullReferenceException) // Missing parameter
                    { }
                }
            } finally {
                if (pipelineRegistered) {
                    // Unregister pipeline
                    serverContext.notificationPipeline
                        .retrieveUserPipeline(user.identifier)
                        .unregister(pipelineHandler);

                    // update playtime (using disconnect time)
                    var metricsService = new UserMetricsService(serverContext, user.identifier);
                    var metricsObject = metricsService.get();
                    metricsObject.playtime += ((ulong) DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) -
                                              metricsObject.lastRealtimeCollection;
                }
            }
        }

        public static async Task acceptWebSocketClientsAsync(HttpContext hc, Func<Task> n, ISContext sctx) {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var ws = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new WebSocketHandler(sctx, ws);
            await h.eventLoopAsync();
        }

        public static void map(IApplicationBuilder app, ISContext context) {
            // DI for websocket handlers
            _realtimeCookieJar = new RealtimeBootstrapper()
                .ConfigureRealtimeHandlerContainer(context);
            app.Use(async (hc, n) => await acceptWebSocketClientsAsync(hc, n, context));
        }
    }
}