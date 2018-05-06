using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Game.Misc {
    public class RoomImageRenderer {
        public Image<Rgba32> drawRoom(Room room) {
            var image = new Image<Rgba32>(Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE);
            for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                    image[x, y] = room.tiles[x, y].getColor();
                }
            }

            return image;
        }

        public Image<Rgba32> drawMap(WorldMap map) {
            var worldMaxX = int.MinValue;
            var worldMaxY = int.MinValue;
            var worldMinX = int.MaxValue;
            var worldMinY = int.MaxValue;
            foreach (var room in map.rooms.Values) {
                if (room.x > worldMaxX) worldMaxX = room.x;
                if (room.x < worldMinX) worldMinX = room.x;

                if (room.y > worldMaxY) worldMaxY = room.y;
                if (room.y < worldMinY) worldMinY = room.y;
            }

            var image = new Image<Rgba32>((Math.Abs((worldMaxX - worldMinX) + 1) * Room.MAP_EDGE_SIZE),
                (Math.Abs((worldMaxY - worldMinY) + 1) * Room.MAP_EDGE_SIZE));

            foreach (var room in map.rooms.Values) {
                var roomX = Room.MAP_EDGE_SIZE * (room.x - worldMinX);
                var roomY = Room.MAP_EDGE_SIZE * (room.y - worldMinY);
                for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                    for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                        image[roomX + x, roomY + y] = map[room.x, room.y].tiles[x, y].getColor();
                    }
                }
            }

            return image;
        }
    }
}