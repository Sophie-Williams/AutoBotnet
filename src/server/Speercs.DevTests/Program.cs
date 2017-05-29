using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Game.MapGen;
using System;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Game.MapGen.Tiles;

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
            
            var room = ServerContext.AppState.WorldMap[0, 0] = generator.GenerateRoom(0, 0);
            room.Print();
            Console.WriteLine();
            
            var start = new RoomPosition(room, 0, room.WestExit.Low + 1);
            var goal  = new RoomPosition(room, room.SouthExit.Low + 1, Room.MapEdgeSize-1);
            var path = start.PathTo(ServerContext, goal);
            if (path == null) {
                Console.WriteLine("no path found");
            } else {
                foreach (var pt in path) {
                    room.Tiles[pt.X, pt.Y] = new TileNRGOre();
                }
                room.Print();
                Console.WriteLine("path length: "+path.Count);
            }
        }

        public static ISContext ServerContext;
    }
}
