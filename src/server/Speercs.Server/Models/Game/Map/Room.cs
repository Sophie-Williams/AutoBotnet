using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Models.Game.Entities;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Map {
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

        public string dump() {
            var sb = new StringBuilder();
            for (var y = 0; y < MAP_EDGE_SIZE; y++) {
                for (var x = 0; x < MAP_EDGE_SIZE; x++) {
                    sb.Append(tiles[x, y].getTileChar());
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public int x { get; }

        public int y { get; }

        public Point spawn;

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