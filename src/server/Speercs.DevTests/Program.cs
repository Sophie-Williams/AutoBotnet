using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Game.MapGen;
using System;
using System.Linq;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Models.Game;
using System.Threading.Tasks;
using Speercs.Server.Models.Game.Program;
using IridiumJS;
using Speercs.Server.Services.Game;

namespace Speercs.DevTests {
    internal static class Program {
        public static void Main(string[] args) {
            var task = mainAsync(args);
            task.Wait();
            Console.WriteLine("PROGRAM DONE");
        }

        private const string js_source = @"
            function loop(x) {
		return x * x;
	    }

            console.log('code load');
        ";

        private const string userId = "foooooo";

        private static async Task setupStuffAsync() {
            serverContext.connectDatabase();

            Console.WriteLine("userID: " + userId);

            var persistentDataService = new PlayerPersistentDataService(serverContext);

            await serverContext.executors.playerPersistentData.createPersistentDataAsync(userId);
            serverContext.executors.playerPersistentData.deployProgram(userId, new UserProgram(js_source));
        }

        private static async Task mainAsync(string[] args) {
            Console.WriteLine("Initializing");

            serverContext = new SContext(new SConfiguration()) {
                appState = new SAppState()
            };
            new BuiltinPluginBootstrapper(serverContext).loadAll();
            await setupStuffAsync();

            Console.WriteLine("Starting test");


            // JS engine testing
            var engine = serverContext.executors.retrieveExecutor(userId).engine; // can be any JSEngine

            // Console.WriteLine("INVOKING loop");

            // const int delay = 10;
            // for (var i = 0; i < 30; i++)
            // {
            //     await Task.Delay(delay);
            //     try
            //     {
            //         Console.WriteLine($"invoking (~{(i + 1) * delay} ms elapsed)");
            //         Console.WriteLine($"  loop({i}) returned: " + engine.Invoke("loop", i));
            //     }
            //     catch (TimeoutException)
            //     {
            //         Console.WriteLine("  [TimeoutException]");
            //     }
            // }

            Console.WriteLine("Testing blockingWait. (With timeout tasks!)");
            var timeLimit = 500;
            var lolEngine = new JSEngine(cfg => {
                cfg.LimitRecursion(4);
            });
            var blockingWait = new Action<int>((time) => System.Threading.Thread.Sleep(time));
            lolEngine.SetValue("blockingWait", blockingWait);
            lolEngine.Execute(@"
function wat(t) {
    blockingWait(t)
}
                ");
            for (var i = 0; i < 10; i++) {
                var ttaken = i * 100;
                Console.Write($"Delay: {ttaken}...");
                try {
                    var executeTask = Task.Run(() => lolEngine.Execute($"blockingWait({ttaken})"));
                    if (executeTask ==
                        await Task.WhenAny(executeTask, Task.Delay(TimeSpan.FromMilliseconds(timeLimit)))) {
                        await executeTask;
                    } else
                        throw new TimeoutException();

                    Console.WriteLine($"Code finished. [{i}]");
                } catch (TimeoutException) {
                    Console.WriteLine($"Code timeout [{i}]");
                }
            }


            Console.WriteLine("DONE");
        }

        public static SContext serverContext;
    }
}