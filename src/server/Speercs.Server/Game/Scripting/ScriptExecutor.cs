using System.Collections.Concurrent;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting {
    public class ScriptExecutor : DependencyObject {
        public ScriptExecutor(JSEngine engine, string userIdentifier, ISContext context) : base(context) {
            Engine = engine;
            UserIdentifier = userIdentifier;
        }

        public BotApi GetBotObject(Bot bot) {
            return botObjects.GetOrAdd(bot.ID, key => {
                return new BotApi(this, bot);
            });
        }

        public bool RemoveBot(Bot bot) {
            return RemoveBot(bot.ID);
        }

        public bool RemoveBot(string botId) {
            return botObjects.TryRemove(botId, out var removed);
        }

        public RoomApi GetRoomObject(int roomX, int roomY) {
            return roomObjects.GetOrAdd($"{roomX}:{roomY}", key => {
                return new RoomApi(this, ServerContext.AppState.WorldMap[roomX, roomY]);
            });
        }

        public JSEngine Engine { get; }
        public string UserIdentifier { get; }

        private ConcurrentDictionary<string, BotApi> botObjects = new ConcurrentDictionary<string, BotApi>();
        private ConcurrentDictionary<string, RoomApi> roomObjects = new ConcurrentDictionary<string, RoomApi>();
    }
}