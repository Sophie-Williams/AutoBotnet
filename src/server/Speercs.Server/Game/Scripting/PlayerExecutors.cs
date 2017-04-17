using IridiumJS;
using Speercs.Server.Services.Game;
using Speercs.Server.Game.Scripting.Api;

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

        public JSEngine RetrieveExecutor(string userIdentifier)
        {
            if (!PlayerEngines.ContainsKey(userIdentifier))
            {
                var engine = new SScriptingHost(ServerContext).CreateSandboxedEngine();

                // load player code into engine
                var playerSource = PlayerPersistentData[userIdentifier].Program.Source;
                engine.Execute(playerSource);

                PlayerEngines.TryAdd(userIdentifier, engine);
            } 
            return UserPipelines[userIdentifier];
        }
    }
}