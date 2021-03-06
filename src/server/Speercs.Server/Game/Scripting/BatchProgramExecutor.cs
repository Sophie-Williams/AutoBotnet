using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Engine;
using Speercs.Server.Services.Application;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Game.Scripting {
    public class BatchProgramExecutor : DependencyObject {
        private UserManagerService _userManager;

        public BatchProgramExecutor(ISContext context) : base(context) {
            _userManager = new UserManagerService(context);
        }

        public async Task executePlayerProgramsAsync() {
            try {
                serverContext.appState.tickCount++;
                // retrieve executors and randomize turn order
                var executors = _userManager.getUsers()
                    .Select(x => serverContext.executors.retrieveExecutor(x.identifier))
                    .OrderBy(a => Guid.NewGuid())
                    .ToList();
                foreach (var executor in executors) {
                    if (executor == null) continue;
                    await Task.Run(() => {
                        var completionWait = new ManualResetEventSlim();
                        var executionHost =
                            new ScriptExecutionHost(executor.engine, executor.userIdentifier, completionWait);
                        var executionThread = new Thread(executionHost.execute);
                        executionThread.Start();
                        completionWait.Wait();
                        try {
                            switch (executionHost.exception) {
                                case null:
                                    serverContext.log.writeLine(
                                        $"Player {executor.userIdentifier} program executed successfully with result {executionHost.result}",
                                        SpeercsLogger.LogLevel.Trace);
                                    break;
                                case TimeoutException ex: {
                                    throw new CodeExecutionException(
                                        $"Player {executor.userIdentifier} code took too long", ex);
                                }
                                case OutOfMemoryException ex: {
                                    throw new CodeExecutionException(
                                        $"Player {executor.userIdentifier} program killed for exceeding memory limit",
                                        ex);
                                }
                                case Exception ex: {
                                    throw new CodeExecutionException(
                                        $"Error executing player {executor.userIdentifier} program", ex);
                                }
                            }
                        } catch (Exception ex) {
                            // generic error
                            serverContext.log.writeLine(ex.Message, SpeercsLogger.LogLevel.Warning);
                            serverContext.log.writeLine(ex.ToString(), SpeercsLogger.LogLevel.Trace);
                        }
                    });
                }
            } catch (Exception ex) {
                // generic error
                serverContext.log.writeLine(ex.ToString(), SpeercsLogger.LogLevel.Error);
            }
        }
    }
}