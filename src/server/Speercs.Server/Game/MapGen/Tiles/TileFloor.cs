using ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileFloor : ITile {
        public bool isWalkable() => true;

        public bool isMinable() => false;

        public char getTileChar() => '.';

        public Rgba32 getColor() => Rgba32.LightGray;
    }
}