using Speercs.Server.Game.MapGen;
using System;
using Speercs.Server.Configuration;

namespace Speercs.DevTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing");

            var config = new SConfiguration();
            ServerContext = new SContext(config);

            Console.WriteLine("Starting mapgen test");

            var generator = new MapGenerator(ServerContext);
            var room = generator.GenerateRoom();
            room.Print();
        }

        public static ISContext ServerContext;
    }
}
