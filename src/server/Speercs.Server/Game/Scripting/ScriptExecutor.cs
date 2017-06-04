using System.Collections.Generic;
using IridiumJS;
using Speercs.Server.Game.Scripting.Api;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting
{
    public class ScriptExecutor
    {
        public ScriptExecutor(JSEngine engine)
        {
            Engine = engine;
        }
        
        public BotAPI GetBotObject(Bot bot)
        {
            if (!botObjects.ContainsKey(bot.ID))
                return botObjects[bot.ID] = new BotAPI(Engine, bot);
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
        
        public JSEngine Engine { get; }
        
        private Dictionary<string, BotAPI> botObjects { get; set; }
    }
}