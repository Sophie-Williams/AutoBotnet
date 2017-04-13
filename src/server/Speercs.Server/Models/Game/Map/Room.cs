using System;
using Newtonsoft.Json;

namespace Speercs.Server.Models.Game.Map
{
    public enum TileType
    {
        Floor,
        Wall,
        Bedrock
    }

    public class Room
    {
        [JsonIgnore]
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
                    return 'O';

                case TileType.Bedrock:
                    return '#';

                default:
                    return '?';
            }
        }

        public void Print()
        {
            for (var y = 0; y < MapEdgeSize; y++)
            {
                for (var x = 0; x < MapEdgeSize; x++)
                {
                    Console.Write(GetTileChar(Tiles[x, y]));
                }
                Console.WriteLine();
            }
        }

        [JsonProperty("tiles")]
        public TileType[,] Tiles { get; }
        public Exit NorthExit, SouthExit, EastExit, WestExit;


        public struct Exit
        {
            public int low { get; }
            public int high { get; }
            public Exit(int l, int h) {
                low = l;
                high = h;
            }
        }
    }
}