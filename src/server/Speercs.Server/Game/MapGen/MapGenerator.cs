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

        public Room GenerateRoom(double density)
        {
            var room = new Room();

            // fill with initial randomness
            for (var x = 0; x < Room.MapEdgeSize; x++)
            {
                for (var y = 0; y < Room.MapEdgeSize; y++)
                {
                    const double halfSize = (Room.MapEdgeSize-1)/2.0;
                    var dx = Math.Abs(x - halfSize);
                    var dy = Math.Abs(y - halfSize);
                    var d = Math.Max(dx, dy) / halfSize;
                    d = density + Math.Pow(d+0.01, 20)*(1-density);
                    room.Tiles[x, y] = rand.NextDouble() < d ? TileType.Wall : TileType.Floor;
                }
            }

            // apply cellular automata for "caves"
            ApplyCellularAutomaton(12);

            // fill in bedrock
            bool ShouldBeBedrock(int x, int y) {
                for (var x2 = x-2; x2 <= x+2; x2++)
                {
                    for (var y2 = y-2; y2 <= y+2; y2++)
                    {
                        if (GetTileAt(x2, y2) == TileType.Floor)
                            return false;
                    }
                }
                return true;
            }
            for (var x = 0; x < Room.MapEdgeSize; x++)
            {
                for (var y = 0; y < Room.MapEdgeSize; y++)
                {
                    if (ShouldBeBedrock(x, y)) room.Tiles[x, y] = TileType.Bedrock;
                }
            }

            return room;

            /// helper functions ///

            TileType GetTileAt(int x, int y) {
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= Room.MapEdgeSize) x = Room.MapEdgeSize-1;
                if (y >= Room.MapEdgeSize) y = Room.MapEdgeSize-1;
                return room.Tiles[x, y];
            }

            void ApplyCellularAutomaton(int numGenerations) {
                var counts = new int[Room.MapEdgeSize, Room.MapEdgeSize];
                for (var generation = 0; generation < numGenerations; generation++)
                {
                    // count each cell's neighbors
                    for (var x = 0; x < Room.MapEdgeSize; x++)
                    {
                        for (var y = 0; y < Room.MapEdgeSize; y++)
                        {
                            counts[x, y] = 0;
                            if (GetTileAt(x-1, y) == TileType.Wall) counts[x, y]++;
                            if (GetTileAt(x+1, y) == TileType.Wall) counts[x, y]++;
                            if (GetTileAt(x, y-1) == TileType.Wall) counts[x, y]++;
                            if (GetTileAt(x, y+1) == TileType.Wall) counts[x, y]++;
                            if (GetTileAt(x-1, y-1) == TileType.Wall) counts[x, y]++;
                            if (GetTileAt(x-1, y+1) == TileType.Wall) counts[x, y]++;
                            if (GetTileAt(x+1, y-1) == TileType.Wall) counts[x, y]++;
                            if (GetTileAt(x+1, y+1) == TileType.Wall) counts[x, y]++;
                        }
                    }

                    // update the grid
                    for (var x = 0; x < Room.MapEdgeSize; x++)
                    {
                        for (var y = 0; y < Room.MapEdgeSize; y++)
                        {
                            if (GetTileAt(x, y) == TileType.Wall)
                            {
                                if (counts[x, y] < 4) room.Tiles[x, y] = TileType.Floor;
                            }
                            else
                            {
                                if (counts[x, y] >= 5) room.Tiles[x, y] = TileType.Wall;
                            }
                        }
                    }
                }
            }
        }

        protected Random rand;
    }
}