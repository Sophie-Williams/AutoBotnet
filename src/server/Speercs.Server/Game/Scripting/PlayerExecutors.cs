using System;
using System.Collections.Concurrent;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting {
    public class PlayerExecutors : DependencyObject {
        // cache of player executors
        ConcurrentDictionary<string, ScriptExecutor> executors { get; } =
            new ConcurrentDictionary<string, ScriptExecutor>();

        public PlayerPersistentDataService playerPersistentData { get; }

        public PlayerExecutors(ISContext context) : base(context) {
            playerPersistentData = new PlayerPersistentDataService(context);
        }

        public ScriptExecutor reloadExecutor(string userIdentifier) {
            if (executors.TryRemove(userIdentifier, out var removed)) {
                return retrieveExecutor(userIdentifier);
            }

            return null;
        }

        public ScriptExecutor retrieveExecutor(string userIdentifier) {
            return executors.GetOrAdd(userIdentifier, key => {
                var engine = new SScriptingHost(serverContext).createSandboxedEngine(userIdentifier);

                // Load player code into engine
                try {
                    var playerSource = playerPersistentData[userIdentifier].program.source;
                    var result = engine.Execute(playerSource).GetCompletionValue();
                } catch (Exception ex) {
                    // Invalid code (syntax or other error on load)
                    // TODO: let the user know
                }

                return new ScriptExecutor(engine, userIdentifier, serverContext);
            });
        }
    }
}