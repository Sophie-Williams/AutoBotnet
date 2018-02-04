using SixLabors.ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileBedrock : ITile {
        public bool isWalkable() => false;

        public bool isMinable() => false;

        public char getTileChar() => '#';

        public Rgba32 getColor() => Rgba32.DarkGray;
    }
}