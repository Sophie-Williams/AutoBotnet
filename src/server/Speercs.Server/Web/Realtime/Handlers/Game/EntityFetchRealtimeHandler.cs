using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Web.Realtime.Handlers {
    public class EntityFetchRealtimeHandler : RealtimeHandler {
        public EntityFetchRealtimeHandler(ISContext context) : base(context, "fetchentity") { }

        public override Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext) {
            var dataBundle = (JObject) data;
            // TODO: ...???
            // switch should handle sending result
            return Task.FromResult<JToken>(JValue.CreateNull());
        }
    }
}