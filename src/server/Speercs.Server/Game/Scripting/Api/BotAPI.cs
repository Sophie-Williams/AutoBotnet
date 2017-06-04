using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Descriptors;
using IridiumJS.Runtime.Interop;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting.Api
{
    public class BotAPI : ObjectInstance
    {
        public BotAPI(JSEngine engine, Bot bot) : base(engine)
        {
            Bot = bot;
            
            FastSetProperty("pos", new PropertyDescriptor(
                new GetterFunctionInstance(engine, thisObj => {
                    return null; // TODO: RoomPositionAPI
                }),
                null,
                true, false
            ));
        }
        
        public override string Class
        {
            get { return "Bot"; }
        }
        
        public Bot Bot { get; }
    }
}