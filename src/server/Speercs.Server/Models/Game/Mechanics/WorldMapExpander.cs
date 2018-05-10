using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Mechanics {
    public class WorldMapExpander : DependencyObject {
        private readonly MapGenerator generator;

        public WorldMapExpander(ISContext context, MapGenerator generator) : base(context) {
            this.generator = generator; }

        public Room createConnectedRoom() {
            var rooms = serverContext.appState.worldMap.rooms.Values.OrderBy(x => x.creationTime);
            var newRoomPos = default(Point);
            foreach (var room in rooms) {
                // look for an open neighbor
                var north = new Point(room.x, room.y - 1);
                if (serverContext.appState.worldMap.get(north) == null) {
                    newRoomPos = north;
                    break;
                }
                var east = new Point(room.x + 1, room.y);
                if (serverContext.appState.worldMap.get(east) == null) {
                    newRoomPos = east;
                    break;
                }
                var south = new Point(room.x, room.y + 1);
                if (serverContext.appState.worldMap.get(south) == null) {
                    newRoomPos = south;
                    break;
                }
                var west = new Point(room.x - 1, room.y);
                if (serverContext.appState.worldMap.get(west) == null) {
                    newRoomPos = west;
                    break;
                }
            }

            var newRoom = generator.generateRoom(newRoomPos.x, newRoomPos.y);
            serverContext.appState.worldMap[newRoomPos.x, newRoomPos.y] = newRoom;

            return newRoom;
        }
    }
}