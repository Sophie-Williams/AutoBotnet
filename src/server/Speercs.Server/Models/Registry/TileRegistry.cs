using System;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Map;

namespace Speercs.Server.Models.Registry {
    public class TileRegistry : DependencyObject, IThingRegistry {
        public TileRegistry(ISContext context) : base(context) { }

        public Type[] tileTypes;

        public int tileId(Tile tile) {
            var tileType = tile.GetType();
            for (var i = 0; i < tileTypes.Length; i++) {
                if (tileType == tileTypes[i]) return i;
            }

            return -1;
        }

        public Type tileById(int id) {
            return tileTypes[id];
        }

        public void recache() {
            tileTypes = serverContext.extensibilityContainer.resolveTypes<Tile>().ToArray();
        }
    }
}