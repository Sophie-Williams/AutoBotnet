using System;
using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Extensibility.MapGen
{
    public class OreFeature : IMapGenFeature
    {
        public enum Location
        {
            Wall,
            ExposedWall,
            UnexposedWall
        }
        
        public OreFeature(Func<ITile> tileGen, double density, Location loc, int veinSize)
            : this(tileGen, density, loc, veinSize, veinSize) {}
        public OreFeature(Func<ITile> tileGen, double density, Location loc, int minVein, int maxVein)
        {
            tileGenerator = tileGen;
            this.density = density;
            location = loc;
            minVeinSize = minVein;
            maxVeinSize = maxVein;
        }
        
        public void Generate(Room room, IMapGenerator generator)
        {
            var points = location==Location.Wall?        generator.Walls :
                         location==Location.ExposedWall? generator.ExposedWalls :
                                                         generator.UnexposedWalls;
            var numVeins = (int)(points.Count * density);
            for (var n = 0; n < numVeins; n++)
            {
                var usedPoints = new HashSet<Point>();
                var candidates = new HashSet<Point>();
                var veinSize = generator.Random.Next(minVeinSize, maxVeinSize+1);
                for (var n2 = 0; n2 < veinSize; n2++)
                {
                    var nextPts = n2==0? points : candidates;
                    if (nextPts.Count == 0) break; // no more room for the vein
                    
                    // choose the point for the next ore tile
                    var point = nextPts.ElementAt(generator.Random.Next(nextPts.Count));
                    
                    // place the ore tile
                    room.Tiles[point.X, point.Y] = tileGenerator();
                    
                    // add new candidates
                    AddCandidate(point.X-1, point.Y);
                    AddCandidate(point.X+1, point.Y);
                    AddCandidate(point.X, point.Y-1);
                    AddCandidate(point.X, point.Y+1);
                    points.Remove(point);     // remove from global locations
                    candidates.Remove(point); // remove from candidates
                    usedPoints.Add(point);    // add to already-used points
                    
                    void AddCandidate(int x, int y)
                    {
                        var pt = new Point(x, y);
                        if (usedPoints.Contains(pt)) return;
                        if (!points.Contains(pt)) return;
                        candidates.Add(pt);
                    }
                }
            }
        }
        
        protected Func<ITile> tileGenerator;
        protected double density;
        protected Location location;
        protected int minVeinSize, maxVeinSize;
    }
}