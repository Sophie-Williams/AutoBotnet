using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileFloor : ITile {
        public bool isWalkable() => true;
        public bool isMinable() => false;
        public char getTileChar() => '.';
        public Rgba32 getColor() => Rgba32.LightGray;
        public string name { get; set; } = "floor";
    }
}