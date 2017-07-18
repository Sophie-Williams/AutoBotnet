using IridiumJS.Native.Object;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class RoomAPI : ObjectInstance
    {
        public RoomAPI(ScriptExecutor executor, Room room) : base(executor.Engine)
        {
            Room = room;
            
            // TODO: properties, methods...
            GameAPI.SetDefaultToString(this);
        }
        
        public override string Class {
            get { return "Room"; }
        }
        
        public Room Room { get; }
    }
}