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
            return GetBotObject(bot.ID);
        }
        
        public BotAPI GetBotObject(string botID)
        {
            if (!botObjects.ContainsKey(botID))
                return botObjects[botID] = new BotAPI(Engine);
            return botObjects[botID];
        }
        
        public JSEngine Engine { get; }
        
        private Dictionary<string, BotAPI> botObjects { get; set; }
    }
}