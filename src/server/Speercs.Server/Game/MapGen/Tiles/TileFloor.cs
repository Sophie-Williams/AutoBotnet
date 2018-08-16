using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Extensibility.Map;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileFloor : Tile {
        public override bool walkable => true;
        public override bool mineable => false;
        public override char tileChar => '.';
        public override Rgba32 color => Rgba32.LightGray;
        public override string name => "floor";
    }
}