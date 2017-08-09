using ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles
{
    public class TileNRGOre : ITile
    {
        public bool IsWalkable() => false;

        public bool IsMinable() => true;

        public char GetTileChar() => '*';

        public Rgba32 GetColor() => Rgba32.Gold;
    }
}
