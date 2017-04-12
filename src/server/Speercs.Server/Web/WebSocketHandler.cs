using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Speercs.Server.Web
{
    public class WebSocketHandler
    {
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
                if (c == '\n') return data;
                data += c;
            }
        }

        public async Task EventLoop()
        {
            while (_ws.State == WebSocketState.Open)
            {
                var data = await ReadLineAsync();
                
            }
        }

        public static async Task AcceptWebSocketClients(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var ws = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new WebSocketHandler(ws);
            await h.EventLoop();
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(WebSocketHandler.AcceptWebSocketClients);
        }
    }
}