using Speercs.Server.Game.MapGen;
using System;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Extensibility.MapGen;

namespace Speercs.DevTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing");
            
            var config = new SConfiguration();
            ServerContext = new SContext(config)
            {
                AppState = new SAppState()
            };
            ServerContext.ExtensibilityContainer.Register<IMapGenFeature>(new TestOre());
            
            Console.WriteLine("Starting mapgen test");
            
            var generator = new MapGenerator(ServerContext);
            var room = generator.GenerateRoom(0, 0);
            room.Print();
        }
        
        public static ISContext ServerContext;
    }
    
    internal class TestOre : IMapGenFeature
    {
        void IMapGenFeature.Generate(Room room, IMapGenerator generator)
        {
            Console.WriteLine("TestOre generate");
            for (var n = 0; n < 25; n++)
            {
                var point = generator.RandomExposedWall();
                room.Tiles[point.X, point.Y] = TileType.Ore;
                generator.ExposedWalls.Remove(point);
            }
            for (var n = 0; n < 10; n++)
            {
                var point = generator.RandomUnexposedWall();
                room.Tiles[point.X, point.Y] = TileType.RareOre;
                generator.UnexposedWalls.Remove(point);
            }
        }
    }
}
