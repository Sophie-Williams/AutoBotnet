using System;
using System.Threading.Tasks;
using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Utilities;

namespace Speercs.DevTests {
    internal static class Program {
        private static string userId = StringUtils.secureRandomString(6);

        private static async Task bootstrapSpeercsAsync() {
            serverContext.connectDatabase();
            Console.WriteLine("userId: " + userId);
        }

        public static async Task Main(string[] args) {
            Console.WriteLine("initializing speercs");

            serverContext = new SContext(new SConfiguration()) {
                appState = new SAppState()
            };
            new BuiltinPluginBootstrapper(serverContext).loadAll();
            await bootstrapSpeercsAsync();

            Console.WriteLine("starting test");
            testRoomGenerator();
        }

        private static void testRoomGenerator() {
            var mapGenerator = new MapGenerator(serverContext, new MapGenParameters());
            var room = mapGenerator.generateRoom(0, 0);
            Console.WriteLine(room.dump());
        }

        public static SContext serverContext;
    }
}