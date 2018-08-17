using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Game.MapGen.Tiles;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Mechanics {
    public class RoomTileInteraction : DependencyObject {
        private UserTeam team;

        public RoomTileInteraction(ISContext context, UserTeam team) : base(context) {
            this.team = team;
        }

        public bool drill(RoomPosition roomPos, int power) {
            var room = roomPos.getRoom(serverContext);
            var tile = room.getTile(roomPos.pos);
            if (!tile.mineable) return false;
            if (power < tile.durability) return false;

            // call Tile::drill for custom behavior/get resources
            var drillContext = new Tile.DrillContext(serverContext);
            var exists = tile.drill(drillContext);
            if (!exists) {
                // remove the tile (turn into "Floor")
                room.setTile(roomPos.pos, new TileFloor());
            }

            // distribute the resources
            foreach (var output in drillContext.outputs) {
                team.addResource(output.Key, output.Value);
            }

            return true;
        }
    }
}