using System;
using System.Collections.Concurrent;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Application;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Engine {
    public class ProgramExecutorManager : DependencyObject {
        // cache of player executors
        ConcurrentDictionary<string, ScriptExecutor> executors { get; } =
            new ConcurrentDictionary<string, ScriptExecutor>();

        public PersistentDataService playerPersistentData { get; }

        public ProgramExecutorManager(ISContext context) : base(context) {
            playerPersistentData = new PersistentDataService(context);
        }

        public ScriptExecutor reloadExecutor(string userIdentifier) {
            if (executors.TryRemove(userIdentifier, out var removed)) {
                return retrieveExecutor(userIdentifier);
            }

            return null;
        }

        public ScriptExecutor retrieveExecutor(string userIdentifier) {
            return executors.GetOrAdd(userIdentifier, key => {
                if (playerPersistentData.get(userIdentifier).program == null) return null;

                var engine = new EngineSandboxer(serverContext).createSandboxedEngine(userIdentifier);

                // Load player code into engine
                try {
                    var playerSource = playerPersistentData.get(userIdentifier).program.source;
                    var result = engine.Execute(playerSource).GetCompletionValue();
                } catch (Exception ex) {
                    // Invalid code (syntax or other error on load)
                    serverContext.log.writeLine($"Error loading player {userIdentifier}'s code: {ex.Message}",
                        SpeercsLogger.LogLevel.Warning);
                    // TODO: let the user know through a notification?
                }

                return new ScriptExecutor(engine, userIdentifier, serverContext);
            });
        }
    }
}