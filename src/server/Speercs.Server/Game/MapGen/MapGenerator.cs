using System;
using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Extensibility.MapGen;
using Speercs.Server.Game.MapGen.Tiles;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;
using static Speercs.Server.Game.MapGen.MapGenConstants;

namespace Speercs.Server.Game.MapGen {
    public class MapGenerator : DependencyObject, IMapGenerator {
        public MapGenerator(ISContext context) : base(context) {
            random = new Random();
        }

        public Room generateRoom(int roomX, int roomY) {
            // set the density
            var density = randomDouble(MIN_ROOM_DENSITY, MAX_ROOM_DENSITY);
            return generateRoom(roomX, roomY, density);
        }

        public Room generateRoom(int roomX, int roomY, double density) {
            var room = new Room(roomX, roomY);

            // set exits
            var adjRoom = serverContext.appState.worldMap[roomX, roomY - 1];
            room.northExit = adjRoom == null ? randomExit() : adjRoom.southExit;
            adjRoom = serverContext.appState.worldMap[roomX, roomY + 1];
            room.southExit = adjRoom == null ? randomExit() : adjRoom.northExit;
            adjRoom = serverContext.appState.worldMap[roomX - 1, roomY];
            room.westExit = adjRoom == null ? randomExit() : adjRoom.eastExit;
            adjRoom = serverContext.appState.worldMap[roomX + 1, roomY];
            room.eastExit = adjRoom == null ? randomExit() : adjRoom.westExit;

            // fill with initial randomness
            initRandomness();

            // apply cellular automata for "caves"
            applyCellularAutomaton(CELLULAR_AUTOMATON_ITERATIONS);

            // fill in bedrock and create tile lists for IMapGenFeatures
            for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                    if (shouldBeBedrock(x, y)) room.tiles[x, y] = new TileBedrock();
                    if (getTileAt(x, y) is TileWall) {
                        walls.Add(new Point(x, y));
                        if (isExposed()) exposedWalls.Add(new Point(x, y));
                        else unexposedWalls.Add(new Point(x, y));

                        bool isExposed() {
                            if (getTileAt(x - 1, y) is TileFloor) return true;
                            if (getTileAt(x + 1, y) is TileFloor) return true;
                            if (getTileAt(x, y - 1) is TileFloor) return true;
                            if (getTileAt(x, y + 1) is TileFloor) return true;
                            return false;
                        }
                    }
                }
            }

            // generate map features
            foreach (var feature in serverContext.extensibilityContainer.ResolveAll<IMapGenFeature>()) {
                feature.generate(room, this);
            }

            // clean up and return
            walls.Clear();
            exposedWalls.Clear();
            unexposedWalls.Clear();
            return room;

            /* helper functions */

            ITile getTileAt(int x, int y) {
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                if (x >= Room.MAP_EDGE_SIZE) x = Room.MAP_EDGE_SIZE - 1;
                if (y >= Room.MAP_EDGE_SIZE) y = Room.MAP_EDGE_SIZE - 1;
                return room.tiles[x, y];
            }

            Room.Exit randomExit() {
                var size = random.Next(MIN_EXIT_SIZE, MAX_EXIT_SIZE);
                var pos = random.Next(1, Room.MAP_EDGE_SIZE - size - 1); // random position, not at a corner
                return new Room.Exit(pos, pos + size - 1);
            }

            void initRandomness() {
                // create base density map
                var densityMap = new double[Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE];
                for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                    for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                        const double halfSize = (Room.MAP_EDGE_SIZE - 1) / 2.0;
                        var dx = Math.Abs(x - halfSize);
                        var dy = Math.Abs(y - halfSize);
                        var d = Math.Max(dx, dy) / halfSize;
                        densityMap[x, y] = density +
                                           Math.Pow(d + 0.01, DENSITY_FALLOFF_EXPONENT) * (1 - density);
                    }
                }

                // augment for exits
                for (var a = 0; a < EXIT_CARVE_DEPTH; a++) {
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
                        if (random.NextDouble() < d) room.tiles[x, y] = new TileWall();
                        else room.tiles[x, y] = new TileFloor();
                    }
                }
            }

            void applyCellularAutomaton(int numGenerations) {
                var counts = new int[Room.MAP_EDGE_SIZE, Room.MAP_EDGE_SIZE];
                for (var generation = 0; generation < numGenerations; generation++) {
                    // count each cell's neighbors
                    for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                        for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                            counts[x, y] = 0;
                            if (getTileAt(x - 1, y) is TileWall) counts[x, y]++;
                            if (getTileAt(x + 1, y) is TileWall) counts[x, y]++;
                            if (getTileAt(x, y - 1) is TileWall) counts[x, y]++;
                            if (getTileAt(x, y + 1) is TileWall) counts[x, y]++;
                            if (getTileAt(x - 1, y - 1) is TileWall) counts[x, y]++;
                            if (getTileAt(x - 1, y + 1) is TileWall) counts[x, y]++;
                            if (getTileAt(x + 1, y - 1) is TileWall) counts[x, y]++;
                            if (getTileAt(x + 1, y + 1) is TileWall) counts[x, y]++;
                        }
                    }

                    // update the grid
                    for (var x = 0; x < Room.MAP_EDGE_SIZE; x++) {
                        for (var y = 0; y < Room.MAP_EDGE_SIZE; y++) {
                            if (getTileAt(x, y) is TileWall) {
                                if (counts[x, y] < 4) room.tiles[x, y] = new TileFloor();
                            } else {
                                if (counts[x, y] >= 5) room.tiles[x, y] = new TileWall();
                            }
                        }
                    }
                }
            }

            bool shouldBeBedrock(int x, int y) {
                // only Wall becomes Bedrock
                if (getTileAt(x, y) is TileFloor) return false;
                // edges are bedrock
                if (getTileAt(x, y) is TileWall &&
                    (x == 0 || x == Room.MAP_EDGE_SIZE - 1 || y == 0 || y == Room.MAP_EDGE_SIZE - 1))
                    return true;
                // walls 2 tiles deep become bedrock
                for (var x2 = x - BEDROCK_DEPTH; x2 <= x + BEDROCK_DEPTH; x2++) {
                    for (var y2 = y - BEDROCK_DEPTH; y2 <= y + BEDROCK_DEPTH; y2++) {
                        if (getTileAt(x2, y2) is TileFloor)
                            return false;
                    }
                }

                return true;
            }
        }

        protected double randomDouble(double min, double max) {
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