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

            // set the density
            var density = RandomDouble(0.40, 0.50);

            // set exits
            room.NorthExit = RandomExit();
            room.SouthExit = RandomExit();
            room.EastExit  = RandomExit();
            room.WestExit  = RandomExit();

            // fill with initial randomness
            InitRandomness();

            // apply cellular automata for "caves"
            ApplyCellularAutomaton(12);

            // fill in bedrock
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

            Room.Exit RandomExit() {
                int size = rand.Next(Room.MapEdgeSize/16, Room.MapEdgeSize/2);
                int pos = rand.Next(1, Room.MapEdgeSize-size-1); // random position, not at a corner
                return new Room.Exit(pos, pos+size-1);
            }

            void InitRandomness() {
                // create base density map
                var densityMap = new double[Room.MapEdgeSize, Room.MapEdgeSize];
                for (var x = 0; x < Room.MapEdgeSize; x++)
                {
                    for (var y = 0; y < Room.MapEdgeSize; y++)
                    {
                        const double halfSize = (Room.MapEdgeSize-1)/2.0;
                        var dx = Math.Abs(x - halfSize);
                        var dy = Math.Abs(y - halfSize);
                        var d = Math.Max(dx, dy) / halfSize;
                        densityMap[x, y] = density + Math.Pow(d+0.01, 20)*(1-density);
                    }
                }

                // augment for exits
                const int exitDepth = Room.MapEdgeSize/16;
                for (var a = 0; a < exitDepth; a++)
                {
                    // north exit
                    for (var b = room.NorthExit.low; b <= room.NorthExit.high; b++)
                    {
                        densityMap[b, a] = 0;
                    }
                    // south exit
                    for (var b = room.SouthExit.low; b <= room.SouthExit.high; b++)
                    {
                        densityMap[b, Room.MapEdgeSize-a-1] = 0;
                    }
                    // west exit
                    for (var b = room.WestExit.low; b <= room.WestExit.high; b++)
                    {
                        densityMap[a, b] = 0;
                    }
                    // east exit
                    for (var b = room.EastExit.low; b <= room.EastExit.high; b++)
                    {
                        densityMap[Room.MapEdgeSize-a-1, b] = 0;
                    }
                }
                
                // fill map
                for (var x = 0; x < Room.MapEdgeSize; x++)
                {
                    for (var y = 0; y < Room.MapEdgeSize; y++)
                    {
                        var d = densityMap[x, y];
                        room.Tiles[x, y] = rand.NextDouble() < d ? TileType.Wall : TileType.Floor;
                    }
                }
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

            bool ShouldBeBedrock(int x, int y) {
                // edges are bedrock
                if (GetTileAt(x, y) == TileType.Wall &&
                    (x==0 || x==Room.MapEdgeSize-1 || y==0 || y==Room.MapEdgeSize-1))
                    return true;
                // walls 2 tiles deep become bedrock
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
        }

        protected double RandomDouble(double min, double max) {
            return rand.NextDouble()*(max-min) + min;
        }

        protected Random rand;
    }
}