using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using LiteDB;
using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Models.Events;
using Speercs.Server.Models.Math;
using Speercs.Server.Services.EventPush;

namespace Speercs.Server.Models.Map {
    public class Room {
        [JsonIgnore] public const int MAP_EDGE_SIZE = 64;

        public const string EVENTPUSH_UPDATE = "update";

        public Room(int x, int y) {
            this.x = x;
            this.y = y;
            tiles = new Tile[MAP_EDGE_SIZE, MAP_EDGE_SIZE];
        }

        public string dump() {
            var sb = new StringBuilder();
            for (var y = 0; y < MAP_EDGE_SIZE; y++) {
                for (var x = 0; x < MAP_EDGE_SIZE; x++) {
                    sb.Append(tiles[x, y].tileChar);
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

        [JsonIgnore]
        [BsonField("tiles")]
        public Tile[,] tiles { get; set; }

        public void setTile(Point pos, Tile tile) {
            tiles[pos.x, pos.y] = tile;
        }

        public Tile getTile(Point pos) {
            return tiles[pos.x, pos.y];
        }

        public bool raiseTileChanged(ISContext context, Point tilePos) {
            var tileRoomPosition = new RoomPosition(pos(), tilePos);
            return context.eventPush.pushEvent(EventPushService.EVENTPUSH_ROOM_TILE, EVENTPUSH_UPDATE,
                tileRoomPosition, TileDeltaEvent.pack(context, tileRoomPosition, getTile(tilePos)));
        }

        public static void writeTile(ISContext context, BinaryWriter bw, Tile tile) {
            var tileId = context.registry.tiles.tileId(tile);
            bw.Write(tileId);
            bw.Write(tile.props.table.Count);
            foreach (var prop in tile.props.table) {
                bw.Write(prop.Key);
                bw.Write(prop.Value);
            }
        }

        public static byte[] packTiles(ISContext context, Tile[,] tiles) {
            using (var output = new MemoryStream())
            using (var compressedOutput = new GZipStream(output, CompressionMode.Compress))
            using (var bw = new BinaryWriter(compressedOutput)) {
                bw.Write(MAP_EDGE_SIZE); // map width
                bw.Write(MAP_EDGE_SIZE); // map height
                for (var i = 0; i < MAP_EDGE_SIZE; i++) {
                    for (var j = 0; j < MAP_EDGE_SIZE; j++) {
                        var tile = tiles[i, j];
                        writeTile(context, bw, tile);
                    }
                }

                bw.Flush();
                return output.ToArray();
            }
        }

        public static Tile[,] unpackTiles(ISContext context, byte[] data) {
            var tiles = default(Tile[,]);
            using (var compressedPack = new MemoryStream(data))
            using (var pack = new GZipStream(compressedPack, CompressionMode.Decompress))
            using (var br = new BinaryReader(pack)) {
                var width = br.ReadInt32();
                var height = br.ReadInt32();
                tiles = new Tile[width, height];
                for (var i = 0; i < width; i++) {
                    for (var j = 0; j < height; j++) {
                        var tileId = br.ReadInt32();
                        var tile = (Tile) Activator.CreateInstance(context.registry.tiles.tileById(tileId));
                        var propsCount = br.ReadInt32();
                        for (var p = 0; p < propsCount; p++) {
                            var key = br.ReadInt32();
                            var val = br.ReadInt64();
                            tile.props.table.Add(key, val);
                        }

                        tiles[i, j] = tile;
                    }
                }
            }

            return tiles;
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