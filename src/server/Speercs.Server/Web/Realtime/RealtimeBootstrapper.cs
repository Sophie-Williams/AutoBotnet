using CookieIoC;
using Speercs.Server.Configuration;
using Speercs.Server.Web.Realtime.Handlers;
using Speercs.Server.Web.Realtime.Handlers.Game;

namespace Speercs.Server.Web.Realtime
{
    public class RealtimeBootstrapper
    {
        public CookieJar ConfigureRealtimeHandlerContainer(ISContext context)
        {
            var container = new CookieJar();
            container.Register<IRealtimeHandler>(new PingRealtimeHandler(context));
            container.Register<IRealtimeHandler>(new MapFetchRealtimeHandler(context));
            container.Register<IRealtimeHandler>(new InteractiveConsoleHandler(context));
            return container;
        }
    }
}
