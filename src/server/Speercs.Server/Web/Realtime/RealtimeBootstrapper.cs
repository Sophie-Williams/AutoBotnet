using CookieIoC;
using Speercs.Server.Configuration;
using Speercs.Server.Web.Realtime.Handlers;

namespace Speercs.Server.Web.Realtime
{
    public class RealtimeBootstrapper
    {
        public CookieJar ConfigureRealtimeHandlerContainer(ISContext context)
        {
            var container = new CookieJar();
            container.Register<IRealtimeHandler>(new PingRealtimeHandler(context));
            return container;
        }
    }
}
