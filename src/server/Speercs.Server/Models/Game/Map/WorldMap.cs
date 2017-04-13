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
                if (!RoomDict.ContainsKey(roomKey)) return null;
                return RoomDict[roomKey];
            }
            set
            {
                var roomKey = $"{x}:{y}";
                RoomDict[roomKey] = value;
            }
        }

        public int RoomCount => RoomDict.Count;
    }
}
