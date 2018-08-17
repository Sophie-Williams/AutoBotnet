using System;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Mechanics {
    public class RoomTileInteraction : DependencyObject {
        protected RoomTileInteraction(ISContext context) : base(context) { }

        public struct DrillResult {
            public bool result;
            public int amount;

            public DrillResult(bool result, int amount) {
                this.result = result;
                this.amount = amount;
            }
        }

        public DrillResult drill(RoomPosition pos, int power) {
            var tile = pos.getTile(serverContext);
            if (tile.mineable && power > tile.durability) {
                // break the tile according to rules
                // TODO: call Tile::drill for custom behavior/get resources
            }
            throw new NotImplementedException();
        }
    }
}