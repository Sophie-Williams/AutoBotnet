using IridiumJS;
using IridiumJS.Native.Object;
using IridiumJS.Runtime;
using IridiumJS.Runtime.Descriptors.Specialized;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class RoomPositionAPI : ObjectInstance
    {
        public RoomPositionAPI(JSEngine engine, RoomPosition pos) : base(engine)
        {
            Position = pos;
            
            FastSetProperty("x", new ClrAccessDescriptor(engine, thisObj => {
                return Position.X;
            }, (thisObj, value) => {
                Position.X = (int)TypeConverter.ToInteger(value);
            }));
            FastSetProperty("y", new ClrAccessDescriptor(engine, thisObj => {
                return Position.Y;
            }, (thisObj, value) => {
                Position.Y = (int)TypeConverter.ToInteger(value);
            }));
            FastSetProperty("room", new ClrAccessDescriptor(engine, thisObj => {
                return null; // todo: RoomAPI
            }, (thisObj, value) => {
                
            }));
        }
        
        public RoomPosition Position;
    }
}