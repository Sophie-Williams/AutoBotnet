using System.Collections.Generic;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Map {
    public class WorldMap {
        public Dictionary<string, Room> roomDict { get; set; } = new Dictionary<string, Room>();

        public Room this[int x, int y] {
            get {
                var roomKey = $"{x}:{y}";
                Room ret;
                roomDict.TryGetValue(roomKey, out ret);
                return ret;
            }
            set {
                var roomKey = $"{x}:{y}";
                var pos = new Point(x, y);
                roomDict[roomKey] = value;
            }
        }

        public int roomCount => roomDict.Count;
    }
}