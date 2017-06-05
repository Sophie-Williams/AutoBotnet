using System.Collections.Concurrent;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting
{
    public class ScriptExecutor : DependencyObject
    {
        public ScriptExecutor(JSEngine engine, ISContext context) : base(context)
        {
            Engine = engine;
        }
        
        public BotAPI GetBotObject(Bot bot)
        {
            return botObjects.GetOrAdd(bot.ID, key => {
                return new BotAPI(this, bot);
            });
        }
        
        public bool RemoveBot(Bot bot)
        {
            return RemoveBot(bot.ID);
        }
        
        public bool RemoveBot(string botID)
        {
            return botObjects.TryRemove(botID, out BotAPI removed);
        }
        
        public RoomAPI GetRoomObject(int roomX, int roomY)
        {
            return roomObjects.GetOrAdd($"{roomX}:{roomY}", key => {
                return new RoomAPI(this, ServerContext.AppState.WorldMap[roomX, roomY]);
            });
        }
        
        public JSEngine Engine { get; }
        
        private ConcurrentDictionary<string, BotAPI> botObjects = new ConcurrentDictionary<string, BotAPI>();
        private ConcurrentDictionary<string, RoomAPI> roomObjects = new ConcurrentDictionary<string, RoomAPI>();
    }
}