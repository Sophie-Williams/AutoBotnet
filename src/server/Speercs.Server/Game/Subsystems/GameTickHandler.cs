using Speercs.Server.Configuration;
using System.Threading.Tasks;
using System;
using System.Linq;
using IridiumJS.Native;
using Speercs.Server.Game.Scripting;

namespace Speercs.Server.Game.Subsystems {
    public class GameTickHandler : DependencyObject {
        public GameTickHandler(ISContext context) : base(context) { }

        public async Task onTickAsync() {
            serverContext.appState.tickCount++;
            var executors =
                serverContext.appState.playerData.Select(x => serverContext.executors.retrieveExecutor(x.Key))
                    .OrderBy(a => Guid.NewGuid()).ToList();
            foreach (var executor in executors) {
                await Task.Run(() => {
                    var engine = executor.engine;
                    try {
                        var loopFunc = engine.GetValue("loop");
                        if (loopFunc != JsValue.Undefined) {
                            var res = loopFunc.Invoke();
                        }
                    } catch (TimeoutException ex) {
                        throw new TimeoutException("Code took too long", ex);
                    } catch (Exception ex) {
                        throw new CodeExecutionException("Error executing player program", ex);
                    }
                });
            }

            // TODO: Tick all entities

            // Game update logic (state: won)
        }
    }
}