using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileWall : ITile {
        public bool isWalkable() => false;
        public bool isMinable() => true;
        public char getTileChar() => 'O';
        public Rgba32 getColor() => Rgba32.Gray;
        public string name { get; set; } = "wall";
    }
}