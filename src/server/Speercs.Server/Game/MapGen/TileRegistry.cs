using System;
using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Game.MapGen.Tiles;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Game.MapGen {
    public static class TileRegistry {
        public static int tileId(ISContext context, ITile tile) {
            var id = 0;
            var tileType = tile.GetType();
            foreach (var existingTileType in context.extensibilityContainer.resolveTypes<ITile>()) {
                if (tileType == existingTileType) {
                    return id;
                } else {
                    id++;
                }
            }
            return -1;
        }

        public static Type tileById(ISContext context, int id) {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
            return context.extensibilityContainer.resolveTypes<ITile>().ElementAt(id);
        }
    }
}