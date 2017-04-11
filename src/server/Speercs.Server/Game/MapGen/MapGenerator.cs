using Speercs.Server.Models.Game.Map;
using System;

namespace Speercs.Server.Game.MapGen
{
    public class MapGenerator
    {
        public MapGenerator()
        {
            rand = new Random();
        }

        public Room GenerateRoom()
        {
            var room = new Room();

            // fill with initial randomness
            for (var x = 0; x < Room.MapEdgeSize; x++)
            {
                for (var y = 0; y < Room.MapEdgeSize; y++)
                {
                    room.Tiles[x, y] = rand.NextDouble() < 0.5 ? TileType.Wall : TileType.Floor;
                }
            }

            // apply cellular automata
            //...

            return room;
        }

        protected Random rand;
    }
}