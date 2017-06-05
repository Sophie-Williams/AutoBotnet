using IridiumJS.Native.Object;
using IridiumJS.Runtime;
using IridiumJS.Runtime.Descriptors.Specialized;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class RoomPositionAPI : ObjectInstance
    {
        public RoomPositionAPI(ScriptExecutor executor, RoomPosition pos) : base(executor.Engine)
        {
            Position = pos;
            
            FastSetProperty("x", new ClrAccessDescriptor(Engine, thisObj => {
                return Position.X;
            }, (thisObj, value) => {
                Position.X = (int)TypeConverter.ToInteger(value);
            }));
            FastSetProperty("y", new ClrAccessDescriptor(Engine, thisObj => {
                return Position.Y;
            }, (thisObj, value) => {
                Position.Y = (int)TypeConverter.ToInteger(value);
            }));
            FastSetProperty("room", new ClrAccessDescriptor(Engine, thisObj => {
                return executor.GetRoomObject(Position.RoomX, Position.RoomY);
            }, (thisObj, value) => {
                var room = value.As<RoomAPI>();
                if (room != null)
                {
                    Position.RoomX = room.Room.X;
                    Position.RoomY = room.Room.Y;
                }
            }));
        }
        
        public RoomPosition Position;
    }
}