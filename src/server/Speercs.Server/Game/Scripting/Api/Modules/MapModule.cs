using System;
using System.Collections.Generic;
using System.Linq;
using IridiumJS;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;
using Speercs.Server.Models.Notifications;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class MapModule : ScriptingApiModule {
        public MapModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            RoomPosition[] findTiles(string name, RoomPosition roomPos) {
                if (name == null) return new RoomPosition[0];
                // retrieve the room
                var room = context.appState.worldMap.get(roomPos);
                var matchedTiles = new List<RoomPosition>();
                for (var i = 0; i < Room.MAP_EDGE_SIZE; i++) {
                    for (var j = 0; j < Room.MAP_EDGE_SIZE; j++) {
                        var tile = room.tiles[i, j];
                        if (tile.name == name) {
                            matchedTiles.Add(new RoomPosition(roomPos.roomPos, new Point(i, j)));
                        }
                    }
                }
                return matchedTiles.ToArray();
            }

            defineFunction(nameof(findTiles), new Func<string, RoomPosition, RoomPosition[]>(findTiles));
        }
    }
}