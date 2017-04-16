using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Game.MapGen;
using System;

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
            new BuiltinPluginBootstrapper(ServerContext).LoadAll();

            Console.WriteLine("Starting mapgen test");

            var generator = new MapGenerator(ServerContext);
            var room = generator.GenerateRoom(0, 0);
            room.Print();
        }

        public static ISContext ServerContext;
    }
}
