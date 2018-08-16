using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Extensibility.Map;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileBedrock : Tile {
        public override bool walkable => false;
        public override bool mineable => false;
        public override char tileChar => '#';
        public override Rgba32 color => Rgba32.DarkGray;
        public override string name => "bedrock";
    }
}