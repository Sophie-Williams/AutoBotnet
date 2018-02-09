using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IridiumJS.Native;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Services.Application;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Subsystems {
    public class GameTickHandler : DependencyObject {
        public GameTickHandler(ISContext context) : base(context) { }

        public async Task onTickAsync() {
            try {
                serverContext.appState.tickCount++;
                var executors =
                    serverContext.appState.userMetrics.Select(x => serverContext.executors.retrieveExecutor(x.Key))
                        .OrderBy(a => Guid.NewGuid()).ToList();
                foreach (var executor in executors) {
                    if (executor == null) continue;
                    await Task.Run(() => {
                        var completionWait = new ManualResetEventSlim();
                        var executionHost = new ScriptExecutionHost(executor.engine, executor.userIdentifier, completionWait);
                        var executionThread = new Thread(executionHost.execute);
                        completionWait.Wait();
                        switch (executionHost.exception) {
                            case null:
                            serverContext.log.writeLine(
                                $"Player {executor.userIdentifier} program executed successfully with result {executionHost.result}",
                                SpeercsLogger.LogLevel.Trace);
                                break;
                            case TimeoutException ex: {
                                throw new TimeoutException($"Player {executor.userIdentifier} code took too long", ex);
                            }
                            case Exception ex: {
                                throw new CodeExecutionException(
                                    $"Error executing player {executor.userIdentifier} program", ex);
                            }
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