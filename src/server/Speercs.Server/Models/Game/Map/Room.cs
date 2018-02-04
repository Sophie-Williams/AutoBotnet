using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Speercs.Server.Extensibility;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Models.Game.Map {
    public class Room {
        [JsonIgnore] public const int MAP_EDGE_SIZE = 64;

        public Room(int x, int y) {
            this.x = x;
            this.y = y;
            tiles = new ITile[MAP_EDGE_SIZE, MAP_EDGE_SIZE];
        }

        public void addEntity(GameEntity entity) {
            entities.Add(entity.id, entity);
        }

        public void removeEntity(GameEntity entity) {
            entities.Remove(entity.id);
        }

        public void print() {
            for (var y = 0; y < MAP_EDGE_SIZE; y++) {
                for (var x = 0; x < MAP_EDGE_SIZE; x++) {
                    Console.Write(tiles[x, y].getTileChar());
                }

                Console.WriteLine();
            }
        }

        public int x { get; }

        public int y { get; }

        [JsonProperty("tiles")]
        public ITile[,] tiles { get; }

        public Dictionary<string, GameEntity> entities { get; } = new Dictionary<string, GameEntity>();

        public Exit northExit, southExit, eastExit, westExit;

        public struct Exit {
            public int low { get; }

            public int high { get; }

            public Exit(int low, int high) {
                this.low = low;
                this.high = high;
            }
        }
    }
}