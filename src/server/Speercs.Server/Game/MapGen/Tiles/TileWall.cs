using ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles
{
    public class TileWall : ITile
    {
        public bool IsWalkable() => false;

        public bool IsMinable() => true;

        public char GetTileChar() => 'O';

        public Color GetColor() => Color.Gray;
    }
}
