using SixLabors.ImageSharp.PixelFormats;

namespace Speercs.Server.Extensibility.Map.Tiles {
    public class TileOre : Tile {
        public override bool walkable => false;
        public override bool mineable => true;
        public override char tileChar => '*';
        public override Rgba32 color => Rgba32.Gold;
        public override string name => "ore";

        public static TileOre create(OreTypes ore, int amount) {
            var tile = new TileOre();
            tile.props.set(PROP_ORE_TYPE, (int) ore);
            tile.props.set(PROP_ORE_AMOUNT, amount);
            return tile;
        }

        public enum OreTypes {
            NRG
        }
    }
}