using System.Collections.Generic;

namespace Speercs.Server.Models.Game.Map
{
    public class WorldMap
    {
        public Dictionary<string, Room> RoomDict { get; set; } = new Dictionary<string, Room>();

        public Room this[int x, int y] => RoomDict[$"{x}:{y}"];

        public int RoomCount => RoomDict.Count;
    }
}
