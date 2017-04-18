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
            
            for (var x = 0; x < 2; x++) {
                for (var y = 0; y < 2; y++) {
                    Console.WriteLine(x+", "+y);
                    var room = ServerContext.AppState.WorldMap[x, y] = generator.GenerateRoom(x, y);
                    room.Print();
                }
            }
        }

        public static ISContext ServerContext;
    }
}
