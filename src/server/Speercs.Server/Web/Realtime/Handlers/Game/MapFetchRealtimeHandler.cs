using System.Collections.Generic;
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
            var command = dataBundle["command"] as JValue;
            if (command == null) return Task.FromResult<JToken>(JValue.CreateNull());
            switch (command.ToString()) {
                case "fetch":
                    var loc = dataBundle["pos"];
                    var x = loc["x"].ToObject<int>();
                    var y = loc["y"].ToObject<int>();
                    // ensure that the user can "see" the room
                    if (!serverContext.appState.entities.getByRoom(new Point(x, y))
                        .Any(nt => nt.teamId == rtContext.userId)) {
                            // fail
                            break;
                    }
                    var room = serverContext.appState.worldMap[x, y];
                    if (room == null) break;
                    return Task.FromResult<JToken>(JObject.FromObject(room));
                case "rooms":
                    // return all rooms with entities owned by [me]
                    var roomPositions = new HashSet<Point>();
                    foreach (var entity in serverContext.appState.entities.getByUser(rtContext.userId)) {
                        roomPositions.Add(entity.position.roomPos);
                    }
                    return Task.FromResult<JToken>(JToken.FromObject(roomPositions.ToArray()));
            }
            // switch should handle sending result
            return Task.FromResult<JToken>(JValue.CreateNull());
        }
    }
}