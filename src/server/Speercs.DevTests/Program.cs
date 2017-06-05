using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Game.MapGen;
using System;
using System.Linq;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Models.Game;
using System.Threading.Tasks;
using Speercs.Server.Models.Game.Program;

namespace Speercs.DevTests
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var task = MainAsync(args);
            task.Wait();
            Console.WriteLine("PROGRAM DONE");
        }
        
        private const string jsSource = @"
            function loop(x) {
                return x * x;
            }
            
            console.log('code load');
        ";
        private const string userID = "foooooo";

        private static async Task setupStuffAsync()
        {
            ServerContext.ConnectDatabase();
            
            Console.WriteLine("userID: " + userID);
            ServerContext.AppState.PlayerData[userID] = new UserTeam
            {
                UserIdentifier = userID
            };

            await ServerContext.Executors.PlayerPersistentData.CreatePersistentDataAsync(userID);
            ServerContext.Executors.PlayerPersistentData.DeployProgram(userID, new UserProgram(jsSource));
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Initializing");

            ServerContext = new SContext(new SConfiguration())
            {
                AppState = new SAppState()
            };
            new BuiltinPluginBootstrapper(ServerContext).LoadAll();
            await setupStuffAsync();

            Console.WriteLine("Starting test");



            // JS engine testing
            var engine = ServerContext.Executors.RetrieveExecutor(userID).Engine; // can be any JSEngine

            Console.WriteLine("INVOKING loop");
            
            const int DELAY = 10;
            for (var i = 0; i < 30; i++)
            {
                await Task.Delay(DELAY);
                try
                {
                    Console.WriteLine($"invoking (~{(i+1)*DELAY} ms elapsed)");
                    Console.WriteLine($"  loop({i}) returned: " + engine.Invoke("loop", i));
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("  [TimeoutException]");
                }
            }
            
            Console.WriteLine("DONE");
        }

        public static SContext ServerContext;
    }
}
