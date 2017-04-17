using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.MapGen;
using Speercs.Server.Game.MapGen.Tiles;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using static Speercs.Server.Game.MapGen.MapGenConstants;

namespace Speercs.Server.Game.MapGen
{
    public class MapGenerator : DependencyObject, IMapGenerator
    {
        public MapGenerator(ISContext context) : base(context)
        {
            Random = new Random();
        }

        public Room GenerateRoom(int roomX, int roomY)
        {
            // set the density
            var density = RandomDouble(MinRoomDensity, MaxRoomDensity);
            return GenerateRoom(roomX, roomY, density);
        }

        public Room GenerateRoom(int roomX, int roomY, double density)
        {
            var room = new Room(roomX, roomY);

            // set exits
            var adjRoom = ServerContext.AppState.WorldMap[roomX, roomY - 1];
            room.NorthExit = adjRoom == null ? RandomExit() : adjRoom.SouthExit;
            adjRoom = ServerContext.AppState.WorldMap[roomX, roomY + 1];
            room.SouthExit = adjRoom == null ? RandomExit() : adjRoom.NorthExit;
            adjRoom = ServerContext.AppState.WorldMap[roomX - 1, roomY];
            room.WestExit = adjRoom == null ? RandomExit() : adjRoom.EastExit;
            adjRoom = ServerContext.AppState.WorldMap[roomX + 1, roomY];
            room.EastExit = adjRoom == null ? RandomExit() : adjRoom.WestExit;

            // fill with initial randomness
            InitRandomness();

            // apply cellular automata for "caves"
            ApplyCellularAutomaton(CellularAutomatonIterations);

            // fill in bedrock and create tile lists for IMapGenFeatures
            for (var x = 0; x < Room.MapEdgeSize; x++)
            {
                for (var y = 0; y < Room.MapEdgeSize; y++)
                {
                    if (ShouldBeBedrock(x, y)) room.Tiles[x, y] = new TileBedrock();
                    if (GetTileAt(x, y) is TileWall)
                    {
                        Walls.Add(new Point(x, y));
                        if (IsExposed()) ExposedWalls.Add(new Point(x, y));
                        else UnexposedWalls.Add(new Point(x, y));

                        bool IsExposed()
                        {
                            if (GetTileAt(x - 1, y) is TileFloor) return true;
                            if (GetTileAt(x + 1, y) is TileFloor) return true;
                            if (GetTileAt(x, y - 1) is TileFloor) return true;
                            if (GetTileAt(x, y + 1) is TileFloor) return true;
                            return false;
                        }
                    }
                }
            }

            // generate map features
            foreach (var feature in ServerContext.ExtensibilityContainer.ResolveAll<IMapGenFeature>())
            {
                feature.Generate(room, this);
            }

            // clean up and return
            Walls.Clear();
            ExposedWalls.Clear();
            UnexposedWalls.Clear();
            return room;

            /* helper functions */

            ITile GetTileAt(int x, int y)
            {
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= Room.MapEdgeSize) x = Room.MapEdgeSize - 1;
                if (y >= Room.MapEdgeSize) y = Room.MapEdgeSize - 1;
                return room.Tiles[x, y];
            }

            Room.Exit RandomExit()
            {
                int size = Random.Next(MinExitSize, MaxExitSize);
                int pos = Random.Next(1, Room.MapEdgeSize - size - 1); // random position, not at a corner
                return new Room.Exit(pos, pos + size - 1);
            }

            void InitRandomness()
            {
                // create base density map
                var densityMap = new double[Room.MapEdgeSize, Room.MapEdgeSize];
                for (var x = 0; x < Room.MapEdgeSize; x++)
                {
                    for (var y = 0; y < Room.MapEdgeSize; y++)
                    {
                        const double halfSize = (Room.MapEdgeSize - 1) / 2.0;
                        var dx = Math.Abs(x - halfSize);
                        var dy = Math.Abs(y - halfSize);
                        var d = Math.Max(dx, dy) / halfSize;
                        densityMap[x, y] = density +
                                           Math.Pow(d + 0.01, DensityFalloffExponent) * (1 - density);
                    }
                }

                // augment for exits
                for (var a = 0; a < ExitCarveDepth; a++)
                {
                    // north exit
                    for (var b = room.NorthExit.Low; b <= room.NorthExit.High; b++)
                    {
                        densityMap[b, a] = 0;
                    }
                    // south exit
                    for (var b = room.SouthExit.Low; b <= room.SouthExit.High; b++)
                    {
                        densityMap[b, Room.MapEdgeSize - a - 1] = 0;
                    }
                    // west exit
                    for (var b = room.WestExit.Low; b <= room.WestExit.High; b++)
                    {
                        densityMap[a, b] = 0;
                    }
                    // east exit
                    for (var b = room.EastExit.Low; b <= room.EastExit.High; b++)
                    {
                        densityMap[Room.MapEdgeSize - a - 1, b] = 0;
                    }
                }

                // fill map
                for (var x = 0; x < Room.MapEdgeSize; x++)
                {
                    for (var y = 0; y < Room.MapEdgeSize; y++)
                    {
                        var d = densityMap[x, y];
                        if (Random.NextDouble() < d) room.Tiles[x, y] = new TileWall();
                        else room.Tiles[x, y] = new TileFloor();
                    }
                }
            }

            void ApplyCellularAutomaton(int numGenerations)
            {
                var counts = new int[Room.MapEdgeSize, Room.MapEdgeSize];
                for (var generation = 0; generation < numGenerations; generation++)
                {
                    // count each cell's neighbors
                    for (var x = 0; x < Room.MapEdgeSize; x++)
                    {
                        for (var y = 0; y < Room.MapEdgeSize; y++)
                        {
                            counts[x, y] = 0;
                            if (GetTileAt(x - 1, y) is TileWall) counts[x, y]++;
                            if (GetTileAt(x + 1, y) is TileWall) counts[x, y]++;
                            if (GetTileAt(x, y - 1) is TileWall) counts[x, y]++;
                            if (GetTileAt(x, y + 1) is TileWall) counts[x, y]++;
                            if (GetTileAt(x - 1, y - 1) is TileWall) counts[x, y]++;
                            if (GetTileAt(x - 1, y + 1) is TileWall) counts[x, y]++;
                            if (GetTileAt(x + 1, y - 1) is TileWall) counts[x, y]++;
                            if (GetTileAt(x + 1, y + 1) is TileWall) counts[x, y]++;
                        }
                    }

                    // update the grid
                    for (var x = 0; x < Room.MapEdgeSize; x++)
                    {
                        for (var y = 0; y < Room.MapEdgeSize; y++)
                        {
                            if (GetTileAt(x, y) is TileWall)
                            {
                                if (counts[x, y] < 4) room.Tiles[x, y] = new TileFloor();
                            }
                            else
                            {
                                if (counts[x, y] >= 5) room.Tiles[x, y] = new TileWall();
                            }
                        }
                    }
                }
            }

            bool ShouldBeBedrock(int x, int y)
            {
                // only Wall becomes Bedrock
                if (GetTileAt(x, y) is TileFloor) return false;
                // edges are bedrock
                if (GetTileAt(x, y) is TileWall &&
                    (x == 0 || x == Room.MapEdgeSize - 1 || y == 0 || y == Room.MapEdgeSize - 1))
                    return true;
                // walls 2 tiles deep become bedrock
                for (var x2 = x - BedrockDepth; x2 <= x + BedrockDepth; x2++)
                {
                    for (var y2 = y - BedrockDepth; y2 <= y + BedrockDepth; y2++)
                    {
                        if (GetTileAt(x2, y2) is TileFloor)
                            return false;
                    }
                }
                return true;
            }
        }

        protected double RandomDouble(double min, double max)
        {
            return Random.NextDouble() * (max - min) + min;
        }

        public Random Random { get; }

        public ISet<Point> Walls { get; set; } = new HashSet<Point>();

        public ISet<Point> ExposedWalls { get; set; } = new HashSet<Point>();

        public ISet<Point> UnexposedWalls { get; set; } = new HashSet<Point>();

        public Point RandomWall()
        {
            return Walls.ElementAt(Random.Next(Walls.Count));
        }

        public Point RandomExposedWall()
        {
            return ExposedWalls.ElementAt(Random.Next(ExposedWalls.Count));
        }

        public Point RandomUnexposedWall()
        {
            return UnexposedWalls.ElementAt(Random.Next(UnexposedWalls.Count));
        }
    }
}
