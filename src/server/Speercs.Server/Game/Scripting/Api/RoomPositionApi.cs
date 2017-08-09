using IridiumJS.Native.Object;
using IridiumJS.Runtime;
using IridiumJS.Runtime.Descriptors.Specialized;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class RoomPositionApi : ObjectInstance
    {
        public RoomPositionApi(ScriptExecutor executor, RoomPosition pos) : base(executor.Engine)
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
                var room = value.As<RoomApi>();
                if (room != null)
                {
                    Position.RoomX = room.Room.X;
                    Position.RoomY = room.Room.Y;
                }
            }));
            FastSetProperty("toString", GameApi.MakeFunctionProperty(Engine, () =>
                $"[{Position.X},{Position.Y}; room {Position.RoomX},{Position.RoomY}]"
            ));
        }
        
        public override string Class {
            get { return "RoomPosition"; }
        }
        
        public RoomPosition Position;
    }
}