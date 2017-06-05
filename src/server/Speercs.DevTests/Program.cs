using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Game.MapGen;
using System;
using System.Linq;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Models.Game;

namespace Speercs.DevTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Initializing");

            ServerContext = new SContext(new SConfiguration())
            {
                AppState = new SAppState()
            };
            new BuiltinPluginBootstrapper(ServerContext).LoadAll();
            // ServerContext.ConnectDatabase();
            
            var userID = "foooooooo";
            Console.WriteLine("userID: "+userID);
            ServerContext.AppState.PlayerData[userID] = new UserTeam
            {
                UserIdentifier = userID
            };

            Console.WriteLine("Starting test");

            var generator = new MapGenerator(ServerContext);
            
            var room = ServerContext.AppState.WorldMap[0, 0] = generator.GenerateRoom(0, 0);
            Console.WriteLine();
            
            // JS engine testing
            var engine = new SScriptingHost(ServerContext).CreateSandboxedEngine(userID);
            
            Console.WriteLine("EXECUTING");
            engine.SetValue("log", (Action<object>)Console.WriteLine);
            engine.SetValue("test", new Action(
                () => {
                    Console.WriteLine("'test' called");
                }
            ));
            engine.Execute(@"
                function loop(x) {
                    return x * x;
                }
            ");
            
            Console.WriteLine("INVOKING loop");
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    Console.WriteLine("loop returned: "+engine.Invoke("loop", 5));
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("got a TimeoutException");
                }
            }
            Console.WriteLine("DONE");
        }

        public static SContext ServerContext;
    }
}
