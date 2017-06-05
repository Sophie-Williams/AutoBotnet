using Speercs.Server.Models.Math;
using System.Collections.Generic;

namespace Speercs.Server.Models.Game.Map
{
    public class WorldMap
    {
        public Dictionary<string, Room> RoomDict { get; set; } = new Dictionary<string, Room>();

        public Room this[int x, int y]
        {
            get
            {
                var roomKey = $"{x}:{y}";
                Room ret;
                RoomDict.TryGetValue(roomKey, out ret);
                return ret;
            }
            set
            {
                var roomKey = $"{x}:{y}";
                var pos = new Point(x, y);
                RoomDict[roomKey] = value;
            }
        }

        public int RoomCount => RoomDict.Count;
    }
}
