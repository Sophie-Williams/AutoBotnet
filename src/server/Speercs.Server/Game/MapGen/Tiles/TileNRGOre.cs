using ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles {
    public class TileNrgOre : ITile {
        public bool isWalkable() => false;

        public bool isMinable() => true;

        public char getTileChar() => '*';

        public Rgba32 getColor() => Rgba32.Gold;
    }
}