using ImageSharp;
using Speercs.Server.Models.Game.Map;
using System;

namespace Speercs.Server.Utilities {
    public class RoomImage {
        public Image<Rgba32> drawRoom(Room room) {
            Image<Rgba32> image = new Image<Rgba32>(Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE);
            for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                    image[x, y] = room.tiles[x, y].getColor();
                }
            }

            return image;
        }

        public Image<Rgba32> drawMap(WorldMap map) {
            int worldMaxX = Int32.MinValue;
            int worldMaxY = Int32.MinValue;
            int worldMinX = Int32.MaxValue;
            int worldMinY = Int32.MaxValue;
            foreach (var room in map.roomDict.Values) {
                if (room.x > worldMaxX) worldMaxX = room.x;
                if (room.x < worldMinX) worldMinX = room.x;

                if (room.y > worldMaxY) worldMaxY = room.y;
                if (room.y < worldMinY) worldMinY = room.y;
            }

            Image<Rgba32> image = new Image<Rgba32>((Math.Abs((worldMaxX - worldMinX) + 1) * Room.MAP_EDGE_SIZE),
                (Math.Abs((worldMaxY - worldMinY) + 1) * Room.MAP_EDGE_SIZE));

            image.BackgroundColor(Rgba32.Black);

            foreach (var room in map.roomDict.Values) {
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