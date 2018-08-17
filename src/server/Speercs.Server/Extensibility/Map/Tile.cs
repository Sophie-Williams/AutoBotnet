using System.Collections.Generic;
using SixLabors.ImageSharp.PixelFormats;

namespace Speercs.Server.Extensibility.Map {
    public abstract class Tile {
        public abstract bool walkable { get; }
        public abstract bool mineable { get; }
        public abstract char tileChar { get; }
        public abstract Rgba32 color { get; }
        public abstract string name { get; }
        public Properties props { get; } = new Properties();
        public virtual int durability { get; protected set; } = 1;

        public const int PROP_ORE_TYPE = 0x07;
        public const int PROP_ORE_AMOUNT = 0x08;

        public class Properties {
            public int get(int id) {
                return table.ContainsKey(id) ? table[id] : 0;
            }

            public void set(int id, int value) {
                table[id] = value;
            }

            public Dictionary<int, int> table = new Dictionary<int, int>();
        }
    }
}