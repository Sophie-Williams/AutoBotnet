using System.Collections.Generic;
using ImageSharp;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Utilities
{
    public class RoomImage
    {
        public Image drawRoom(Room room)
        {
            Dictionary<TileType, Color> colorDict = new Dictionary<TileType, Color>();
            colorDict.Add(TileType.Floor, Color.LightGray);
            colorDict.Add(TileType.Wall, Color.Gray);
            colorDict.Add(TileType.Bedrock, Color.DarkGray);
            Image image = new Image(Room.MapEdgeSize, Room.MapEdgeSize);
            using (var pixels = image.Lock())
            {
                for (var y = 0; y < Room.MapEdgeSize; y++)
                {
                    for (var x = 0; x < Room.MapEdgeSize; x++)
                    {
                        pixels[x, y] = colorDict[room.Tiles[x, y]];
                    }
                }
            }
            return image;
        }
    }
}