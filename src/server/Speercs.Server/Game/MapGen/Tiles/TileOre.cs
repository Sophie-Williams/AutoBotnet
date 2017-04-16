using ImageSharp;
using Speercs.Server.Extensibility;

namespace Speercs.Server.Game.MapGen.Tiles
{
    public class TileOre: ITile
    {
        public bool IsWalkable() => true;
        public bool IsMinable() => false;
        
        public char GetTileChar() => '*';
        public Color GetColor() => Color.DodgerBlue;
    }
}