using IridiumJS;
using IridiumJS.Native.Object;
using Speercs.Server.Configuration;

namespace Speercs.Server.Game.Scripting.Api
{
    public class GameAPI : ObjectInstance
    {
        public GameAPI(JSEngine engine, ISContext context, string userID) : base(engine)
        {
            FastAddProperty("bots", new BotsDict(Engine, context, userID), false, true, false);
        }
        
        public override string Class
        {
            get { return "GameAPI"; }
        }
    }
}