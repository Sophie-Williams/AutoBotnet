using CookieIoC;
using Speercs.Server.Configuration;
using Speercs.Server.Web.Realtime.Handlers;

namespace Speercs.Server.Web.Realtime
{
    public class RealtimeBootstrapper
    {
        public CookieContainer ConfigureRealtimeHandlerContainer(ISContext context)
        {
            var container = new CookieContainer();
            container.Register<IRealtimeHandler>(new PingRealtimeHandler(context));
            return container;
        }
    }
}