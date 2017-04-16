using ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles
{
    public class TileBedrock : ITile
    {
        public bool IsWalkable() => false;

        public bool IsMinable() => false;

        public char GetTileChar() => '#';

        public Color GetColor() => Color.DarkGray;
    }
}
