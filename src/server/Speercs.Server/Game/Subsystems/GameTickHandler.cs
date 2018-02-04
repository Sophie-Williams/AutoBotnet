using System;
using System.Linq;
using System.Threading.Tasks;
using IridiumJS.Native;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Services.Application;

namespace Speercs.Server.Game.Subsystems {
    public class GameTickHandler : DependencyObject {
        public GameTickHandler(ISContext context) : base(context) { }

        public async Task onTickAsync() {
            try {
                serverContext.appState.tickCount++;
                var executors =
                    serverContext.appState.playerData.Select(x => serverContext.executors.retrieveExecutor(x.Key))
                        .OrderBy(a => Guid.NewGuid()).ToList();
                foreach (var executor in executors) {
                    await Task.Run(() => {
                        var engine = executor.engine;
                        try {
                            var loopFunc = engine.GetValue("loop");
                            engine.ResetTimeoutTicks();
                            JsValue res = null;
                            if (loopFunc != JsValue.Undefined) {
                                res = loopFunc.Invoke();
                            }

                            serverContext.log.writeLine(
                                $"Player {executor.userIdentifier} program executed successfully with result {res}",
                                SpeercsLogger.LogLevel.Trace);
                        } catch (TimeoutException ex) {
                            throw new TimeoutException($"Player {executor.userIdentifier} code took too long", ex);
                        } catch (Exception ex) {
                            throw new CodeExecutionException(
                                $"Error executing player {executor.userIdentifier} program", ex);
                        }
                    });
                }

                // TODO: Tick all entities

                // Game update logic (state: won)
            } catch (Exception ex) {
                serverContext.log.writeLine(ex.ToString(), SpeercsLogger.LogLevel.Warning);
            }
        }
    }
}