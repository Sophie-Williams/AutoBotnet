using System;
using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.Map;
using Speercs.Server.Game.MapGen.Tiles;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Game.MapGen {
    public class MapGenerator : DependencyObject, IMapGenerator {
        public MapGenParameters prm { get; set; }

        public MapGenerator(ISContext context, MapGenParameters prm) : base(context) {
            this.prm = prm;
            random = new Random();
        }

        public Room generateRoom(int roomX, int roomY) {
            var room = new Room(roomX, roomY);
            room.creationTime = serverContext.appState.tickCount;
            
            // variables
            var density = doubleRange(prm.minRoomDensity, prm.maxRoomDensity);

            // key points
            var center = new Point(Room.MAP_EDGE_SIZE / 2, Room.MAP_EDGE_SIZE / 2);

            // set exits
            var adjRoom = serverContext.appState.worldMap[roomX, roomY - 1];
            room.northExit = adjRoom?.southExit ?? randomExit();
            adjRoom = serverContext.appState.worldMap[roomX, roomY + 1];
            room.southExit = adjRoom?.northExit ?? randomExit();
            adjRoom = serverContext.appState.worldMap[roomX - 1, roomY];
            room.westExit = adjRoom?.eastExit ?? randomExit();
            adjRoom = serverContext.appState.worldMap[roomX + 1, roomY];
            room.eastExit = adjRoom?.westExit ?? randomExit();

            // fill with initial randomness
            fillDensityMap();

            // apply cellular automata for "caves"
            applyCellularAutomaton(prm.cellularAutomatonIterations);

            // fill in bedrock and create tile lists for IMapGenFeatures
            for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                    if (shouldBeBedrock(x, y)) room.tiles[x, y] = new TileBedrock();
                    if (tileAt(x, y) is TileRock) {
                        walls.Add(new Point(x, y));
                        if (isExposed()) exposedWalls.Add(new Point(x, y));
                        else unexposedWalls.Add(new Point(x, y));

                        bool isExposed() {
                            if (tileAt(x - 1, y) is TileFloor) return true;
                            if (tileAt(x + 1, y) is TileFloor) return true;
                            if (tileAt(x, y - 1) is TileFloor) return true;
                            if (tileAt(x, y + 1) is TileFloor) return true;
                            return false;
                        }
                    }
                }
            }

            // generate map features
            foreach (var feature in serverContext.extensibilityContainer.resolveAll<IMapGenFeature>()) {
                feature.generate(room, this);
            }

            // select a spawn point near the center
            // pick an empty tile
            var spawnCandidates = new List<Point>();
            for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                    if (tileAt(x, y) is TileFloor) {
                        spawnCandidates.Add(new Point(x, y));
                    }
                }
            }

            // pick a spawn point
            spawnCandidates = spawnCandidates.OrderBy(x => Point.chDist(x, center)).Take(prm.spawnPointCandidates)
                .ToList();
            room.spawn = spawnCandidates[random.Next(spawnCandidates.Count)];
            room.tiles[room.spawn.x, room.spawn.y] = new TileCrashSite();


            // clean up and return
            walls.Clear();
            exposedWalls.Clear();
            unexposedWalls.Clear();
            return room;

            /* helper functions */

            Tile tileAt(int x, int y) {
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= Room.MAP_EDGE_SIZE) x = Room.MAP_EDGE_SIZE - 1;
                if (y >= Room.MAP_EDGE_SIZE) y = Room.MAP_EDGE_SIZE - 1;
                return room.tiles[x, y];
            }

            Room.Exit randomExit() {
                var size = random.Next(prm.minExitSize, prm.maxExitSize);
                var pos = random.Next(1, Room.MAP_EDGE_SIZE - size - 1); // random position, not at a corner
                return new Room.Exit(pos, pos + size - 1);
            }

            void fillDensityMap() {
                // create base density map
                var densityMap = new double[Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE];
                for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                    for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                        const double halfSize = (Room.MAP_EDGE_SIZE - 1) / 2.0;
                        var dx = Math.Abs(x - halfSize);
                        var dy = Math.Abs(y - halfSize);
                        var d = Math.Max(dx, dy) / halfSize;
                        densityMap[x, y] = density +
                                           Math.Pow(d + 0.01, prm.densityFalloffExponent) * (1 - density);
                    }
                }

                // augment for exits
                for (var a = 0; a < prm.exitCarveDepth; a++) {
                    // north exit
                    for (var b = room.northExit.low; b <= room.northExit.high; b++) {
                        densityMap[b, a] = 0;
                    }

                    // south exit
                    for (var b = room.southExit.low; b <= room.southExit.high; b++) {
                        densityMap[b, Room.MAP_EDGE_SIZE - a - 1] = 0;
                    }

                    // west exit
                    for (var b = room.westExit.low; b <= room.westExit.high; b++) {
                        densityMap[a, b] = 0;
                    }

                    // east exit
                    for (var b = room.eastExit.low; b <= room.eastExit.high; b++) {
                        densityMap[Room.MAP_EDGE_SIZE - a - 1, b] = 0;
                    }
                }

                // fill map
                for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                    for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                        var d = densityMap[x, y];
                        if (random.NextDouble() < d) room.tiles[x, y] = new TileRock();
                        else room.tiles[x, y] = new TileFloor();
                    }
                }
            }

            void applyCellularAutomaton(int generations) {
                var counts = new int[Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE];
                for (var generation = 0; generation < generations; generation++) {
                    // count each cell's neighbors
                    for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                        for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                            counts[x, y] = 0;
                            if (tileAt(x - 1, y) is TileRock) counts[x, y]++;
                            if (tileAt(x + 1, y) is TileRock) counts[x, y]++;
                            if (tileAt(x, y - 1) is TileRock) counts[x, y]++;
                            if (tileAt(x, y + 1) is TileRock) counts[x, y]++;
                            if (tileAt(x - 1, y - 1) is TileRock) counts[x, y]++;
                            if (tileAt(x - 1, y + 1) is TileRock) counts[x, y]++;
                            if (tileAt(x + 1, y - 1) is TileRock) counts[x, y]++;
                            if (tileAt(x + 1, y + 1) is TileRock) counts[x, y]++;
                        }
                    }

                    // update the grid
                    for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                        for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                            if (tileAt(x, y) is TileRock) {
                                if (counts[x, y] < prm.cellularAutomatonMinNeighbors)
                                    room.tiles[x, y] = new TileFloor();
                            } else {
                                if (counts[x, y] >= prm.cellularAutomatonMaxNeighbors)
                                    room.tiles[x, y] = new TileRock();
                            }
                        }
                    }
                }
            }

            bool shouldBeBedrock(int x, int y) {
                // only Wall becomes Bedrock
                if (tileAt(x, y) is TileFloor) return false;
                // edges are bedrock
                if (tileAt(x, y) is TileRock &&
                    (x == 0 || x == Room.MAP_EDGE_SIZE - 1 || y == 0 || y == Room.MAP_EDGE_SIZE - 1))
                    return true;
                // walls 2 tiles deep become bedrock
                for (var x2 = x - prm.bedrockDepth; x2 <= x + prm.bedrockDepth; x2++) {
                    for (var y2 = y - prm.bedrockDepth; y2 <= y + prm.bedrockDepth; y2++) {
                        if (tileAt(x2, y2) is TileFloor)
                            return false;
                    }
                }

                return true;
            }
        }

        protected double doubleRange(double min, double max) {
            return random.NextDouble() * (max - min) + min;
        }

        public Random random { get; }

        public ISet<Point> walls { get; set; } = new HashSet<Point>();

        public ISet<Point> exposedWalls { get; set; } = new HashSet<Point>();

        public ISet<Point> unexposedWalls { get; set; } = new HashSet<Point>();

        public Point randomWall() {
            return walls.ElementAt(random.Next(walls.Count));
        }

        public Point randomExposedWall() {
            return exposedWalls.ElementAt(random.Next(exposedWalls.Count));
        }

        public Point randomUnexposedWall() {
            return unexposedWalls.ElementAt(random.Next(unexposedWalls.Count));
        }
    }
}