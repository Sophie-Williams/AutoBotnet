using ImageSharp;
using Speercs.Server.Models.Game.Map;
using System;

namespace Speercs.Server.Utilities {
    public class RoomImage {
        public Image<Rgba32> drawRoom(Room room) {
            Image<Rgba32> image = new Image<Rgba32>(Room.MapEdgeSize, Room.MapEdgeSize);
            for (var y = 0; y < Room.MapEdgeSize; y++) {
                for (var x = 0; x < Room.MapEdgeSize; x++) {
                    image[x, y] = room.Tiles[x, y].GetColor();
                }
            }

            return image;
        }

        public Image<Rgba32> drawMap(WorldMap map) {
            int worldMaxX = Int32.MinValue;
            int worldMaxY = Int32.MinValue;
            int worldMinX = Int32.MaxValue;
            int worldMinY = Int32.MaxValue;
            foreach (var room in map.RoomDict.Values) {
                if (room.X > worldMaxX) worldMaxX = room.X;
                if (room.X < worldMinX) worldMinX = room.X;

                if (room.Y > worldMaxY) worldMaxY = room.Y;
                if (room.Y < worldMinY) worldMinY = room.Y;
            }

            Image<Rgba32> image = new Image<Rgba32>((Math.Abs((worldMaxX - worldMinX) + 1) * Room.MapEdgeSize),
                (Math.Abs((worldMaxY - worldMinY) + 1) * Room.MapEdgeSize));

            image.BackgroundColor(Rgba32.Black);

            foreach (var room in map.RoomDict.Values) {
                var roomX = Room.MapEdgeSize * (room.X - worldMinX);
                var roomY = Room.MapEdgeSize * (room.Y - worldMinY);
                for (var y = 0; y < Room.MapEdgeSize; y++) {
                    for (var x = 0; x < Room.MapEdgeSize; x++) {
                        image[roomX + x, roomY + y] = map[room.X, room.Y].Tiles[x, y].GetColor();
                    }
                }
            }

            return image;
        }
    }
}