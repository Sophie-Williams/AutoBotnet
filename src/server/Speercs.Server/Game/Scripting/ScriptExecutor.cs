using System.Collections.Generic;
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
            if (!botObjects.ContainsKey(bot.ID))
                return botObjects[bot.ID] = new BotAPI(this, bot);
            return botObjects[bot.ID];
        }
        
        public bool RemoveBot(Bot bot)
        {
            return RemoveBot(bot.ID);
        }
        
        public bool RemoveBot(string botID)
        {
            return botObjects.Remove(botID);
        }
        
        public RoomAPI GetRoomObject(int roomX, int roomY)
        {
            var key = $"{roomX}:{roomY}";
            if (!roomObjects.ContainsKey(key))
                return roomObjects[key] = new RoomAPI(this, ServerContext.AppState.WorldMap[roomX, roomY]);
            return roomObjects[key];
        }
        
        public JSEngine Engine { get; }
        
        private Dictionary<string, BotAPI> botObjects = new Dictionary<string, BotAPI>();
        private Dictionary<string, RoomAPI> roomObjects = new Dictionary<string, RoomAPI>();
    }
}