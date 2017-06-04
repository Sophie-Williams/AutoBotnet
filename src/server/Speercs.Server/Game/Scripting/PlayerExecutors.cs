using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Game;
using System.Collections.Concurrent;

namespace Speercs.Server.Game.Scripting
{
    public class PlayerExecutors : DependencyObject
    {
        // cache of player executors
        ConcurrentDictionary<string, ScriptExecutor> Executors { get; } = new ConcurrentDictionary<string, ScriptExecutor>();

        public PlayerPersistentDataService PlayerPersistentData { get; }

        public PlayerExecutors(ISContext context) : base(context)
        {
            PlayerPersistentData = new PlayerPersistentDataService(context);
        }

        public ScriptExecutor ReloadExecutor(string userIdentifier)
        {
            if (Executors.TryRemove(userIdentifier, out ScriptExecutor removed))
            {
                return RetrieveExecutor(userIdentifier);
            }
            return null;
        }

        public ScriptExecutor RetrieveExecutor(string userIdentifier)
        {
            return Executors.GetOrAdd(userIdentifier, key => {
                var engine = new SScriptingHost(ServerContext).CreateSandboxedEngine(userIdentifier);

                // load player code into engine
                try
                {
                    var playerSource = PlayerPersistentData[userIdentifier].Program.Source;
                    engine.Execute(playerSource);
                }
                catch
                {
                    // invalid code (syntax or other error on load)
                    // TODO: let the user know
                }

                return new ScriptExecutor(engine);
            });
        }
    }
}