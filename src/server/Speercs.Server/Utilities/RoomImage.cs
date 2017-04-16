using ImageSharp;
using Speercs.Server.Models.Game.Map;
using System;

namespace Speercs.Server.Utilities
{
    public class RoomImage
    {
        public Image drawRoom(Room room)
        {
            Image image = new Image(Room.MapEdgeSize, Room.MapEdgeSize);
            using (var pixels = image.Lock())
            {
                for (var y = 0; y < Room.MapEdgeSize; y++)
                {
                    for (var x = 0; x < Room.MapEdgeSize; x++)
                    {
                        pixels[x, y] = room.Tiles[x, y].GetColor();
                    }
                }
            }
            return image;
        }

        public Image drawMap(WorldMap map)
        {
            int worldMaxX = Int32.MinValue;
            int worldMaxY = Int32.MinValue;
            int worldMinX = Int32.MaxValue;
            int worldMinY = Int32.MaxValue;
            map.RoomPositions.ForEach((i) =>
            {
                if (i.X > worldMaxX) worldMaxX = i.X;
                if (i.X < worldMinX) worldMinX = i.X;

                if (i.Y > worldMaxY) worldMaxY = i.Y;
                if (i.Y < worldMinY) worldMinY = i.Y;
            });
            Image image = new Image((Math.Abs((worldMaxX - worldMinX) + 1) * Room.MapEdgeSize), (Math.Abs((worldMaxY - worldMinY) + 1) * Room.MapEdgeSize));

            image.BackgroundColor(Color.Black);

            using (var pixels = image.Lock())
            {
                map.RoomPositions.ForEach((i) =>
                {
                    var roomX = Room.MapEdgeSize * (i.X - worldMinX);
                    var roomY = Room.MapEdgeSize * (i.Y - worldMinY);
                    for (var y = 0; y < Room.MapEdgeSize; y++)
                    {
                        for (var x = 0; x < Room.MapEdgeSize; x++)
                        {
                            pixels[roomX + x, roomY + y] = map[i.X, i.Y].Tiles[x, y].GetColor();
                        }
                    }
                });
            }

            return image;
        }
    }
}
