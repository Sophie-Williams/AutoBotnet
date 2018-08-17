using SixLabors.ImageSharp.PixelFormats;

namespace Speercs.Server.Extensibility.Map.Tiles {
    public class TileOre : Tile {
        public override bool walkable => false;
        public override bool mineable => true;
        public override char tileChar => '*';
        public override Rgba32 color => Rgba32.Gold;
        public override string name => "ore";

        public TileOre(int durability) {
            this.durability = durability;
        }

        public static TileOre create(IOreType ore, int amount) {
            var tile = new TileOre(ore.durability);
            tile.props.set(PROP_ORE_TYPE, (int) ore.resourceId);
            tile.props.set(PROP_ORE_AMOUNT, amount);
            return tile;
        }

        public interface IOreType {
            int durability { get; }
            int resourceId { get; }
            string name { get; }
        }

        public class NRGOre : IOreType {
            public int durability => 1;
            public int resourceId => 1;
            public string name => "nrg";
        }
    }
}