using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Game;
using System.Collections.Concurrent;

namespace Speercs.Server.Game.Scripting
{
    public class PlayerExecutors : DependencyObject
    {
        // cache of player engines
        ConcurrentDictionary<string, JSEngine> PlayerEngines { get; } = new ConcurrentDictionary<string, JSEngine>();

        public PlayerPersistentDataService PlayerPersistentData { get; }

        public PlayerExecutors(ISContext context) : base(context)
        {
            PlayerPersistentData = new PlayerPersistentDataService(context);
        }

        public JSEngine ReloadExecutor(string userIdentifier)
        {
            if (PlayerEngines.TryRemove(userIdentifier, out JSEngine removed))
            {
                return RetrieveExecutor(userIdentifier);
            }
            return null;
        }

        public JSEngine RetrieveExecutor(string userIdentifier)
        {
            if (!PlayerEngines.ContainsKey(userIdentifier))
            {
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

                PlayerEngines.TryAdd(userIdentifier, engine);
            } 
            return PlayerEngines[userIdentifier];
        }
    }
}