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
            ServerContext = new SContext(config);
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
        }
    }
}
