using System;
using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Extensibility.Map.Features {
    public class OreFeature : IMapGenFeature {
        public enum Location {
            Wall,
            ExposedWall,
            UnexposedWall
        }

        public OreFeature(Func<Tile> tileGen, double density, Location loc, int veinSize)
            : this(tileGen, density, loc, veinSize, veinSize) { }

        public OreFeature(Func<Tile> tileGen, double density, Location loc, int minVein, int maxVein) {
            tileGenerator = tileGen;
            this.density = density;
            location = loc;
            minVeinSize = minVein;
            maxVeinSize = maxVein;
        }

        public void generate(Room room, IMapGenerator generator) {
            var points = location == Location.Wall ? generator.walls :
                location == Location.ExposedWall ? generator.exposedWalls :
                generator.unexposedWalls;
            var numVeins = (int) (points.Count * density);
            for (var n = 0; n < numVeins; n++) {
                var usedPoints = new HashSet<Point>();
                var candidates = new HashSet<Point>();
                var veinSize = generator.random.Next(minVeinSize, maxVeinSize + 1);
                for (var n2 = 0; n2 < veinSize; n2++) {
                    var nextPts = n2 == 0 ? points : candidates;
                    if (nextPts.Count == 0) break; // no more room for the vein

                    // choose the point for the next ore tile
                    var point = nextPts.ElementAt(generator.random.Next(nextPts.Count));

                    // place the ore tile
                    room.tiles[point.x, point.y] = tileGenerator();

                    // add new candidates
                    addCandidate(point.x - 1, point.y);
                    addCandidate(point.x + 1, point.y);
                    addCandidate(point.x, point.y - 1);
                    addCandidate(point.x, point.y + 1);
                    points.Remove(point); // remove from global locations
                    candidates.Remove(point); // remove from candidates
                    usedPoints.Add(point); // add to already-used points

                    void addCandidate(int x, int y) {
                        var pt = new Point(x, y);
                        if (usedPoints.Contains(pt)) return;
                        if (!points.Contains(pt)) return;
                        candidates.Add(pt);
                    }
                }
            }
        }

        protected Func<Tile> tileGenerator;
        protected double density;
        protected Location location;
        protected int minVeinSize, maxVeinSize;
    }
}