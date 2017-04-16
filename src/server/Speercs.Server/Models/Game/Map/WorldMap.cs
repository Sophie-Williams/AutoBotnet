using Speercs.Server.Models.Math;
using System.Collections.Generic;
using System.Linq;

namespace Speercs.Server.Models.Game.Map
{
    public class WorldMap
    {
        public Dictionary<string, Room> RoomDict { get; set; } = new Dictionary<string, Room>();

        public List<Point> RoomPositions { get; } = new List<Point>();

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
                var pos = new Point(x, y);
                // register room position
                if (!RoomPositions.Where(p => p.EqualTo(pos)).Any())
                {
                    // register room position
                    RoomPositions.Add(pos);
                }
                RoomDict[roomKey] = value;
            }
        }

        public int RoomCount => RoomDict.Count;
    }
}
