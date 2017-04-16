using System;
using System.Linq;
using Speercs.Server.Models.Game.Map;

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
        
        public OreFeature(Func<ITile> tileGen, double density, Location loc)
        {
            tileGenerator = tileGen;
            this.density = density;
            location = loc;
        }
        
        public void Generate(Room room, IMapGenerator generator)
        {
            Console.WriteLine("Walls: "+generator.Walls.Count);
            Console.WriteLine("ExposedWalls: "+generator.ExposedWalls.Count);
            Console.WriteLine("UnexposedWalls: "+generator.UnexposedWalls.Count);
            var points = location==Location.Wall?        generator.Walls :
                         location==Location.ExposedWall? generator.ExposedWalls :
                                                         generator.UnexposedWalls;
            var numVeins = (int)(points.Count * density);
            for (var n = 0; n < numVeins; n++)
            {
                if (points.Count == 0) break; // no more room, for whatever reason
                var point = points.ElementAt(generator.Random.Next(points.Count));
                room.Tiles[point.X, point.Y] = tileGenerator();
                points.Remove(point);
            }
        }
        
        protected Func<ITile> tileGenerator;
        protected double density;
        protected Location location;
    }
}