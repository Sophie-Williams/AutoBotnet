using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;

namespace Speercs.Server.Web.Realtime.Handlers {
    public class PingRealtimeHandler : RealtimeHandler {
        public PingRealtimeHandler(ISContext context) : base(context, "ping") { }

        public override Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext) {
            return Task.FromResult<JToken>(new JValue("pong"));
        }
    }
}