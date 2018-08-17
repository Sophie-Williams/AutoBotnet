using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Configuration;

namespace Speercs.Server.Extensibility.Map {
    public abstract class Tile {
        public abstract bool walkable { get; }
        public abstract bool mineable { get; }
        public abstract char tileChar { get; }
        public abstract Rgba32 color { get; }
        public abstract string name { get; }
        public Properties props { get; } = new Properties();
        public virtual int durability { get; protected set; } = 1;

        public class DrillContext {
            public ISContext serverContext { get; }

            public DrillContext(ISContext serverContext) {
                this.serverContext = serverContext;
            }

            public Dictionary<string, long> outputs = new Dictionary<string, long>();
        }

        /// <summary>
        /// Drill the tile
        /// </summary>
        /// <returns>Whether the tile still exists after drilling</returns>
        public virtual bool drill(DrillContext drillContext) {
            return mineable;
        }

        public class Properties {
            public long get(int id) {
                return table.ContainsKey(id) ? table[id] : 0;
            }

            public void set(int id, long value) {
                table[id] = value;
            }

            public Dictionary<int, long> table = new Dictionary<int, long>();
        }
    }
}