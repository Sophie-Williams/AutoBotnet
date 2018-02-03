using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Web.Realtime.Handlers.Game {
    public class MapFetchRealtimeHandler : RealtimeHandler {
        public MapFetchRealtimeHandler(ISContext context) : base(context, "fetchmap") { }

        public override Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext) {
            var dataBundle = (JObject) data;
            var x = dataBundle["x"].ToObject<int>();
            var y = dataBundle["y"].ToObject<int>();
            var room = serverContext.appState.worldMap[x, y];
            if (room == null) return Task.FromResult<JToken>(JValue.CreateNull());
            return Task.FromResult<JToken>(JObject.FromObject(room));
        }
    }
}