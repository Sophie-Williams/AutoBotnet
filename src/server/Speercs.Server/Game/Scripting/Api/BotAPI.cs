using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Descriptors.Specialized;
using IridiumJS.Runtime.Interop;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting.Api
{
    public class BotAPI : ObjectInstance
    {
        public BotAPI(JSEngine engine, Bot bot) : base(engine)
        {
            Bot = bot;
            
            FastSetProperty("pos", new ClrAccessDescriptor(engine, thisObj => {
                return new RoomPositionAPI(engine, bot.Position);
            }));
        }
        
        public override string Class
        {
            get { return "Bot"; }
        }
        
        public Bot Bot { get; }
    }
}