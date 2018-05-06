using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Extensibility.Map.Tiles {
    public class TileOre : ITile {
        public bool isWalkable() => false;

        public bool isMinable() => true;

        public char getTileChar() => '*';

        public Rgba32 getColor() => Rgba32.Gold;

        public ResourceId resource;
    }
}