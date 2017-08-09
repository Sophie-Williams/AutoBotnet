using System;
using IridiumJS;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Descriptors;
using IridiumJS.Runtime.Interop;
using Speercs.Server.Configuration;

namespace Speercs.Server.Game.Scripting.Api
{
    public class GameApi : ObjectInstance
    {
        public GameApi(JSEngine engine, ISContext context, string userID) : base(engine)
        {
            FastAddProperty("bots", new BotsDict(Engine, context, userID), false, true, false);
            SetDefaultToString(this);
        }
        
        public override string Class
        {
            get { return "GameAPI"; }
        }
        
        public static PropertyDescriptor MakeFunctionProperty<T>(JSEngine engine, Func<T> func)
        {
            return new PropertyDescriptor(new DelegateWrapper(engine, func), false, false, false);
        }
        
        public static void SetDefaultToString(ObjectInstance obj)
        {
            obj.FastSetProperty("toString", MakeFunctionProperty(obj.Engine, () => $"[object {obj.Class}]"));
        }
    }
}