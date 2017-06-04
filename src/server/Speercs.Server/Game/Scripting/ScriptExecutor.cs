using System.Collections.Generic;
using IridiumJS;
using Speercs.Server.Game.Scripting.Api;

namespace Speercs.Server.Game.Scripting
{
    public class ScriptExecutor
    {
        public ScriptExecutor(JSEngine engine)
        {
            Engine = engine;
        }
        
        public JSEngine Engine { get; }
        
        public Dictionary<string, BotAPI> BotObjects { get; set; }
    }
}