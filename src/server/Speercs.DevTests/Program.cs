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
using Speercs.Server.Models.Game.Entities;
using Speercs.Server.Game.Scripting;

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
            var engine = new SScriptingHost(ServerContext).CreateSandboxedEngine("<userid>");
            
            Console.WriteLine("EXECUTING");
            engine.SetValue("log", (Action<object>)Console.WriteLine);
            engine.SetValue("test", new Action<Direction>(
                x => {
                    Console.WriteLine("it werkd: "+x);
                }
            ));
            engine.Execute(@"
                function loop() {
                    var kek = 2;
                    log(kek + 3, 'tickles');
                    
                    log();
                    test(0);
                    test(1);
                    test(2);
                    log(EAST);
                    log(typeof EAST);
                    test(6);
                    log();
                    
                    return 'corn?';
                }
            ");
            Console.WriteLine(engine.Invoke("loop"));
            Console.WriteLine("DONE");

            Console.WriteLine("kekloop");
            try
            {
                engine.Execute(@"
                    for (let kek = 0;;kek++) { }
                ");
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
