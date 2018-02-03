using ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileWall : ITile {
        public bool isWalkable() => false;

        public bool isMinable() => true;

        public char getTileChar() => 'O';

        public Rgba32 getColor() => Rgba32.Gray;
    }
}