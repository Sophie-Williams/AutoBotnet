using System;
using System.Collections.Concurrent;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting {
    public class PlayerExecutors : DependencyObject {
        // cache of player executors
        ConcurrentDictionary<string, ScriptExecutor> Executors { get; } =
            new ConcurrentDictionary<string, ScriptExecutor>();

        public PlayerPersistentDataService PlayerPersistentData { get; }

        public PlayerExecutors(ISContext context) : base(context) {
            PlayerPersistentData = new PlayerPersistentDataService(context);
        }

        public ScriptExecutor ReloadExecutor(string userIdentifier) {
            if (Executors.TryRemove(userIdentifier, out ScriptExecutor removed)) {
                return RetrieveExecutor(userIdentifier);
            }

            return null;
        }

        public ScriptExecutor RetrieveExecutor(string userIdentifier) {
            return Executors.GetOrAdd(userIdentifier, key => {
                var engine = new SScriptingHost(ServerContext).CreateSandboxedEngine(userIdentifier);

                // Load player code into engine
                try {
                    var playerSource = PlayerPersistentData[userIdentifier].Program.Source;
                    var result = engine.Execute(playerSource).GetCompletionValue();
                } catch (Exception ex) {
                    // Invalid code (syntax or other error on load)
                    // TODO: let the user know
                }

                return new ScriptExecutor(engine, userIdentifier, ServerContext);
            });
        }
    }
}