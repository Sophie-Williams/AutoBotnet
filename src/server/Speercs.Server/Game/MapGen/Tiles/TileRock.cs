using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Extensibility.Map;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileRock : Tile {
        public override bool walkable => false;
        public override bool mineable => true;
        public override char tileChar => 'O';
        public override Rgba32 color => Rgba32.Gray;
        public override string name => "rock";
    }
}