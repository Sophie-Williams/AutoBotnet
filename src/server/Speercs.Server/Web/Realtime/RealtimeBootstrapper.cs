using CookieIoC;
using Speercs.Server.Configuration;
using Speercs.Server.Web.Realtime.Handlers;

namespace Speercs.Server.Web.Realtime {
    public class RealtimeBootstrapper {
        public CookieJar ConfigureRealtimeHandlerContainer(ISContext context) {
            var container = new CookieJar();
            container.register<IRealtimeHandler>(new PingRealtimeHandler(context));
            container.register<IRealtimeHandler>(new MapFetchRealtimeHandler(context));
            container.register<IRealtimeHandler>(new EntityFetchRealtimeHandler(context));
            container.register<IRealtimeHandler>(new InteractiveConsoleHandler(context));
            return container;
        }
    }
}