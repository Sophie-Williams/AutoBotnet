using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Web.Realtime.Handlers {
    public class MapFetchRealtimeHandler : RealtimeHandler {
        public MapFetchRealtimeHandler(ISContext context) : base(context, "fetchmap") { }

        public override Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext) {
            var dataBundle = (JObject) data;
            var x = dataBundle["x"].ToObject<int>();
            var y = dataBundle["y"].ToObject<int>();
            // ensure that the user can "see" the room
            if (!serverContext.appState.entities.getByRoom(new Point(x, y))
                .Any(nt => nt.teamId == rtContext.userIdentifier)) {
                    // fail
                    return Task.FromResult<JToken>(JValue.CreateNull());
            }
            var room = serverContext.appState.worldMap[x, y];
            if (room == null) return Task.FromResult<JToken>(JValue.CreateNull());
            return Task.FromResult<JToken>(JObject.FromObject(room));
        }
    }
}