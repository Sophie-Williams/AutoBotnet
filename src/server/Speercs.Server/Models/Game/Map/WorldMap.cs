using System.Collections.Generic;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Map {
    public class WorldMap {
        public Dictionary<string, Room> rooms { get; set; } = new Dictionary<string, Room>();

        public Room this[int x, int y] {
            get {
                var roomKey = $"{x}:{y}";
                Room ret;
                rooms.TryGetValue(roomKey, out ret);
                return ret;
            }
            set {
                var roomKey = $"{x}:{y}";
                var pos = new Point(x, y);
                rooms[roomKey] = value;
            }
        }

        public Room get(Point roomPos) {
            return this[roomPos.x, roomPos.y];
        }

        public int roomCount => rooms.Count;
    }
}