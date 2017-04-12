using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Threading;
using Speercs.Server.Web.Realtime;
using Speercs.Server.Configuration;
using Newtonsoft.Json.Linq;
using CookieIoC;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Speercs.Server.Web
{
    public class WebSocketHandler
    {
        private static CookieContainer realtimeCookieContainer;
        private WebSocket _ws;
        public WebSocketHandler(WebSocket websocket) 
        {
            _ws = websocket;
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
            while (_ws.State == WebSocketState.Open)
            {
                var rawData = await ReadLineAsync();
                var requestBundle = JObject.Parse(rawData);
                try
                {
                    await HandleRequestAsync(requestBundle)
                        .ContinueWith(async t =>
                        {
                            // send result
                            var resultBundle = (JObject)t.Result;
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

        public async Task<JToken> HandleRequestAsync(JObject requestBundle)
        {
            // parse request
            var rcommand = ((JValue)requestBundle["request"]).ToObject<string>();
            var data = (JObject)requestBundle["data"];
            var id = ((JValue)requestBundle["id"]).ToObject<long>();
            // get handler
            var handler = realtimeCookieContainer.GetAll<IRealtimeHandler>().FirstOrDefault(x => x.Path == rcommand);
            return await handler?.HandleRequestAsync(id, data);
        }

        public static async Task AcceptWebSocketClientsAsync(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var ws = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new WebSocketHandler(ws);
            await h.EventLoopAsync();
        }

        public static void Map(IApplicationBuilder app, ISContext context)
        {
            // DI for websocket handlers
            realtimeCookieContainer = new RealtimeBootstrapper()
                .ConfigureRealtimeHandlerContainer(context);
            app.Use(WebSocketHandler.AcceptWebSocketClientsAsync);
        }
    }
}