using IridiumJS.Native.Object;
using IridiumJS.Runtime.Descriptors.Specialized;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting.Api
{
    public class BotAPI : ObjectInstance
    {
        public BotAPI(ScriptExecutor executor, Bot bot) : base(executor.Engine)
        {
            Bot = bot;
            
            FastSetProperty("pos", new ClrAccessDescriptor(Engine, thisObj => {
                return new RoomPositionAPI(executor, Bot.Position);
            }));
        }
        
        public override string Class
        {
            get { return "Bot"; }
        }
        
        public Bot Bot { get; }
    }
}