using SixLabors.ImageSharp.PixelFormats;

namespace Speercs.Server.Extensibility.Map.Tiles {
    public class TileOre : ITile {
        public bool isWalkable() => false;
        public bool isMinable() => true;
        public char getTileChar() => '*';
        public Rgba32 getColor() => Rgba32.Gold;
        public string name { get; set; } = "ore";
        public string resource;
    }
}