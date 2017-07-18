using Speercs.Server.Configuration;
using System.Threading.Tasks;
using System;

namespace Speercs.Server.Game.Subsystems
{
    public class GameTickHandler : DependencyObject
    {
        public GameTickHandler(ISContext context) : base(context)
        {
        }

        public async Task OnTickAsync()
        {
            ServerContext.AppState.TickCount++;
            
            foreach (var entry in ServerContext.AppState.PlayerData)
            {
                await Task.Run(() => 
                {
                    var engine = ServerContext.Executors.RetrieveExecutor(entry.Value.UserIdentifier).Engine;
                    try
                    {
                        var loopFunc = engine.GetValue("loop");
                        Console.WriteLine("loopFunc: "+loopFunc);
                        var res = loopFunc.Invoke(5);
                        Console.WriteLine("returned: "+res);
                        // engine.Invoke("loop");
                    }
                    catch (TimeoutException)
                    {
                        // code timed out
                    }
                    catch
                    {
                        // thrown if 'loop' isn't a function:
                        //   System.ArgumentException: Can only invoke functions
                    }
                });
            }
        }
    }
}