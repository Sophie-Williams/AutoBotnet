using IridiumJS.Native.Object;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class RoomApi : ObjectInstance
    {
        public RoomApi(ScriptExecutor executor, Room room) : base(executor.Engine)
        {
            Room = room;
            
            // TODO: properties, methods...
            GameApi.SetDefaultToString(this);
        }
        
        public override string Class {
            get { return "Room"; }
        }
        
        public Room Room { get; }
    }
}