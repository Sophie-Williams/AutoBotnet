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

        private async Task<string> readLineAsync() {
            var data = string.Empty;
            while (true) {
                var buf = new byte[1];
                var arraySeg = new ArraySegment<byte>(buf);
                await _ws.ReceiveAsync(arraySeg, CancellationToken.None);
                var c = (char) buf[0];
                data += c;
                if (c == '\n') return data;
            }
        }

        private async Task writeLineAsync(string data) {
            await _ws.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        public async Task eventLoopAsync() {
            async Task<(JToken, string, long)> handleRequestAsync(JObject requestBundle) {
                // Parse request
                var rcommand = ((JValue) requestBundle["request"]).ToObject<string>();
                var data = (JObject) requestBundle["data"];
                var id = ((JValue) requestBundle["id"]).ToObject<long>();
                // Get handler
                var handler = _realtimeCookieJar.ResolveAll<IRealtimeHandler>().FirstOrDefault(x => x.path == rcommand);
                var result = (JToken) JValue.CreateNull();
                if (handler != null) {
                    result = await handler.handleRequestAsync(id, data, _rtContext);
                }

                return (result, rcommand, id);
            }

            var pipelineRegistered = false;
            var currentUser = default(RegisteredUser);
            var pipelineHandler = new Func<JObject, Task<bool>>(async bundle => {
                await writeLineAsync(bundle.ToString(Formatting.None));
                return false;
            });
            try {
                // Require auth first
                var authApiKey = await readLineAsync();
                // Attempt to authenticate
                if (!_rtContext.authenticateWith(authApiKey)) {
                    await writeLineAsync("false"); // authentication failure
                    await _ws.CloseAsync(WebSocketCloseStatus.ProtocolError, "Invalid authentication key",
                        CancellationToken.None);
                    throw new SecurityException();
                }

                await writeLineAsync("true"); // websocket channel was authenticated successfully
                currentUser = await _rtContext.getCurrentUserAsync();
                // Attempt to add handler
                serverContext.notificationPipeline
                    .retrieveUserPipeline(currentUser.identifier)
                    .addItemToEnd(pipelineHandler);
                pipelineRegistered = true;

                // save last connection metric
                var metricsObject = serverContext.appState.userMetrics[currentUser.identifier];
                metricsObject.lastConnection =
                    (ulong) DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                // pipeline is registered, send any queued, previously undelivered data
                var userNotificationQueue =
                    new PlayerPersistentDataService(serverContext).retrieveNotificationQueue(currentUser.identifier);
                while (userNotificationQueue.Count > 0) {
                    // send data back through pipelines
                    // now that a pipeline is registered, the data will be sent through the channel
                    // in the case of an exception or other delivery failure, the message will be queued again.
                    await serverContext.notificationPipeline.pushMessageAsync(userNotificationQueue.Dequeue(),
                        currentUser.identifier);
                }

                while (_ws.State == WebSocketState.Open) {
                    var rawData = await readLineAsync();
                    var requestBundle = JObject.Parse(rawData);
                    try {
                        await handleRequestAsync(requestBundle)
                            .ContinueWith(async t => {
                                (var response, var request, var id) = t.Result;
                                // Send result
                                var resultBundle = new JObject(
                                    new JProperty("id", id),
                                    new JProperty("data", response),
                                    new JProperty("type", "response"),
                                    new JProperty("request", request)
                                );
                                // Write result to websocket
                                await writeLineAsync(resultBundle.ToString(Formatting.None));
                            });
                    } catch (NullReferenceException) // Missing parameter
                    { }
                }
            } finally {
                if (pipelineRegistered) {
                    // Unregister pipeline
                    serverContext.notificationPipeline
                        .retrieveUserPipeline(currentUser.identifier)
                        .UnregisterHandler(pipelineHandler);

                    // update playtime (using disconnect time)
                    var metricsObject = serverContext.appState.userMetrics[currentUser.identifier];
                    metricsObject.playtime += ((ulong) DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) -
                                              metricsObject.lastConnection;
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