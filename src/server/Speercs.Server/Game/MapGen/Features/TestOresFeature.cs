using System;
using Speercs.Server.Extensibility.MapGen;
using Speercs.Server.Game.MapGen.Tiles;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.MapGen.Features
{
    public class TestOresFeature : IMapGenFeature
    {
        void IMapGenFeature.Generate(Room room, IMapGenerator generator)
        {
            Console.WriteLine("TestOre generate");
            for (var n = 0; n < 25; n++)
            {
                var point = generator.RandomExposedWall();
                room.Tiles[point.X, point.Y] = new TileOre();
                generator.ExposedWalls.Remove(point);
            }
            for (var n = 0; n < 10; n++)
            {
                var point = generator.RandomUnexposedWall();
                room.Tiles[point.X, point.Y] = new TileRareOre();
                generator.UnexposedWalls.Remove(point);
            }
        }
    }
}