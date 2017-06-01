using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Game.MapGen;
using System;
using Speercs.Server.Models.Game.Map;
using Speercs.Server.Game.MapGen.Tiles;
using IridiumJS;
using IridiumJS.Runtime.Interop;
using IridiumJS.Native;
using IridiumJS.Runtime;

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

            Console.WriteLine("Starting test");

            var generator = new MapGenerator(ServerContext);
            
            var room = ServerContext.AppState.WorldMap[0, 0] = generator.GenerateRoom(0, 0);
            Console.WriteLine();
            
            // JS engine testing
            var engine = new JSEngine(
                cfg =>
                {
                    cfg.LimitRecursion(10);
                    cfg.TimeoutInterval(TimeSpan.FromMilliseconds(500));
                }
            );
            
            Console.WriteLine("EXECUTING");
            engine.SetValue("log", new Action<string>(
                (s) => Console.WriteLine(s)
            ));
            engine.Execute(@"
                function loop() {
                    // log = 'toast';
                    
                    var kek = 2;
                    log(kek + 3, 'tickles');
                    return 'corn?';
                }
            ");
            Console.WriteLine(engine.Invoke("loop"));
            Console.WriteLine("DONE");

            Console.WriteLine("kekloop");
            var timeoutEngine = new JSEngine(
                cfg =>
                {
                    cfg.LimitRecursion(10);
                    cfg.TimeoutInterval(TimeSpan.FromMilliseconds(500));
                }
            );
            try
            {
                timeoutEngine.Execute(@"
                    function loop() {
                        for (let kek = 0;;kek++) { }
                    }
                ");
                Console.WriteLine(timeoutEngine.Invoke("loop"));
            }
            catch (TimeoutException)
            {
                Console.WriteLine("kek ran out of time kek");
            }
            Console.WriteLine("DONE");
        }

        public static ISContext ServerContext;
    }
}
