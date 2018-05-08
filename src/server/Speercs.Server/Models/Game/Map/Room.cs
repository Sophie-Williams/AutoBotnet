using System.Collections.Generic;
using System.Text;
using LiteDB;
using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Map {
    public class Room {
        [JsonIgnore] public const int MAP_EDGE_SIZE = 64;

        public Room(int x, int y) {
            this.x = x;
            this.y = y;
            tiles = new ITile[MAP_EDGE_SIZE, MAP_EDGE_SIZE];
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

        [BsonField("x")]
        public int x { get; set; }

        [BsonField("y")]
        public int y { get; set; }

        public Point pos() => new Point(x, y);

        [BsonField("spawn")]
        public Point spawn { get; set; }

        [BsonField("creationTime")]
        public ulong creationTime { get; set; }

        [JsonProperty("tiles")]
        [BsonField("tiles")]
        public ITile[,] tiles { get; set; }

        public static PackedTile[] packTiles(ISContext context, ITile[,] tiles) {
            var arr = new PackedTile[MAP_EDGE_SIZE * MAP_EDGE_SIZE];
            for (var i = 0; i < MAP_EDGE_SIZE; i++) {
                for (var j = 0; j < MAP_EDGE_SIZE; j++) {
                    var tile = tiles[i, j];
                    // TODO: pack tile props
                    arr[i * MAP_EDGE_SIZE + j] = new PackedTile(TileRegistry.tileId(context, tile));
                }
            }
            return arr;
        }

        public struct PackedTile {
            public int id { get; set; }
            public Dictionary<string, object> props { get; set; }

            public PackedTile(int id, Dictionary<string, object> props = null) {
                this.id = id;
                if (props != null)
                    this.props = props;
                else
                    this.props = new Dictionary<string, object>();
            }
        }

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