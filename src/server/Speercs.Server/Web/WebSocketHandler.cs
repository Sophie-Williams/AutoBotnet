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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                // parse request
                var rcommand = ((JValue)requestBundle["request"]).ToObject<string>();
                var data = (JObject)requestBundle["data"];
                var id = ((JValue)requestBundle["id"]).ToObject<long>();
                // get handler
                var handler = realtimeCookieJar.ResolveAll<IRealtimeHandler>().FirstOrDefault(x => x.Path == rcommand);
                return (await handler?.HandleRequestAsync(id, data, rtContext), rcommand, id);
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
                            // send result
                            var resultBundle = new JObject(
                                new JProperty("id", id),
                                new JProperty("data", response),
                                new JProperty("type", "response"),
                                new JProperty("request", request)
                            );
                            // write result to websocket
                            await WriteLineAsync(resultBundle.ToString(Formatting.None));
                        });
                }
                catch (NullReferenceException) // Missing parameter
                {
                    continue;
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
