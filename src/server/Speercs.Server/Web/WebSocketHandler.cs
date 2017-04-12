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

        public async Task EventLoop()
        {
            while (_ws.State == WebSocketState.Open)
            {
                var rawData = await ReadLineAsync();
                var requestBundle = JObject.Parse(rawData);
                await HandleRequest(requestBundle)
                    .ContinueWith(t =>
                    {
                        // send result
                        var resultBundle = (JObject)t.Result;
                        // TODO: write result to websocket
                    });
            }
        }

        public async Task<JToken> HandleRequest(JObject requestBundle)
        {
            // parse request
            var rcommand = ((JValue)requestBundle["request"]).ToObject<string>();
            var data = (JObject)requestBundle["data"];
            var id = ((JValue)requestBundle["id"]).ToObject<long>();
            // get handler
            var handler = realtimeCookieContainer.GetAll<IRealtimeHandler>().FirstOrDefault(x => x.Path == rcommand);
            return await handler?.HandleRequest(id, data);
        }

        public static async Task AcceptWebSocketClients(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var ws = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new WebSocketHandler(ws);
            await h.EventLoop();
        }

        public static void Map(IApplicationBuilder app, ISContext context)
        {
            // DI for websocket handlers
            realtimeCookieContainer = new RealtimeBootstrapper()
                .ConfigureRealtimeHandlerContainer(context);
            app.Use(WebSocketHandler.AcceptWebSocketClients);
        }
    }
}