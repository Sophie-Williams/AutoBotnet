using Newtonsoft.Json;
using Speercs.Server.Extensibility;
using System;
using System.Collections.Generic;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Models.Game.Map
{
    public class Room
    {
        [JsonIgnore]
        public const int MapEdgeSize = 64;

        public Room(int x, int y)
        {
            X = x;
            Y = y;
            Tiles = new ITile[MapEdgeSize, MapEdgeSize];
        }
        
        public void AddEntity(GameEntity entity)
        {
            Entities.Add(entity.ID, entity);
        }
        
        public void RemoveEntity(GameEntity entity)
        {
            Entities.Remove(entity.ID);
        }

        public void Print()
        {
            for (var y = 0; y < MapEdgeSize; y++)
            {
                for (var x = 0; x < MapEdgeSize; x++)
                {
                    Console.Write(Tiles[x, y].GetTileChar());
                }
                Console.WriteLine();
            }
        }

        public int X { get; }

        public int Y { get; }

        [JsonProperty("tiles")]
        public ITile[,] Tiles { get; }
        
        public Dictionary<string, GameEntity> Entities { get; } = new Dictionary<string, GameEntity>();

        public Exit NorthExit, SouthExit, EastExit, WestExit;

        public struct Exit
        {
            public int Low { get; }

            public int High { get; }

            public Exit(int low, int high)
            {
                Low = low;
                High = high;
            }
        }
    }
}
