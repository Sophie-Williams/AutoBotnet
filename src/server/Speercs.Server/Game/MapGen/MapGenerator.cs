using System;
using Speercs.Server.Models.Room;

namespace Speercs.Server.Game.MapGen {

    public class MapGenerator {

        public MapGenerator(){
            rand = new Random();
        }

        public Room GenerateRoom() {
            var room = new Room();

            // fill with initial randomness
            for (var x = 0; x < Room.SIZE; x++) {
                for (var y = 0; y < Room.SIZE; y++) {
                    room.Tiles[x, y] = rand.NextDouble()<0.5? TileType.Wall : TileType.Floor;
                }
            }

            // apply cellular automata
            //...

            return room;
        }

        protected Random rand;

    }

}