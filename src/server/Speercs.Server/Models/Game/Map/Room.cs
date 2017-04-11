using System;

namespace Speercs.Server.Models.Game.Map
{

    public enum TileType
    {
        Floor,
        Wall
    }

    public class Room
    {
        public const int MapEdgeSize = 64;

        public Room()
        {
            Tiles = new TileType[MapEdgeSize, MapEdgeSize];
        }

        public static char GetTileChar(TileType t)
        {
            switch (t)
            {
                case TileType.Floor:
                    return '.';

                case TileType.Wall:
                    return '#';

                default:
                    return '?';
            }
        }

        public void Print()
        {
            for (var x = 0; x < MapEdgeSize; x++)
            {
                for (var y = 0; y < MapEdgeSize; y++)
                {
                    Console.Write(GetTileChar(Tiles[x, y]));
                }
                Console.WriteLine();
            }
        }

        public TileType[,] Tiles { get; }
    }
}