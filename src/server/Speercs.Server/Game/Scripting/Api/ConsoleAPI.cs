using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Interop;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Notifications;
using Newtonsoft.Json.Linq;
using System;

namespace Speercs.Server.Game.Scripting.Api
{
    public class ConsoleAPI : ObjectInstance
    {
        private ISContext ServerContext { get; set; }
        private string UserId { get; set; }
        public ConsoleAPI(JSEngine engine, ISContext serverContext, string userId) : base(engine)
        {
            ServerContext = serverContext;
            UserId = userId;
            FastAddProperty("log", new ClrFunctionInstance(Engine, Log, 0), false, true, false);
            FastAddProperty("notify", new ClrFunctionInstance(Engine, Notify, 0), false, true, false);
            GameAPI.SetDefaultToString(this);
        }
        
        public override string Class
        {
            get { return "Console"; }
        }
        
        private JsValue Log(JsValue thisObj, JsValue[] args)
        {
            string str = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0) str += " ";
                str += args[i].ToString();
            }
            
            var notif = JObject.FromObject(new PushNotification("log", str));
            // TODO: Figure out why this times out
            ServerContext.EventQueue.QueuePush(notif, UserId);
            return JsValue.Undefined;
        }

        private JsValue Notify(JsValue thisObj, JsValue[] args)
        {
            string str = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0) str += " ";
                str += args[i].ToString();
            }
            
            var notif = JObject.FromObject(new PushNotification("notif", str));
            // TODO: Figure out why this times out
            ServerContext.EventQueue.QueuePush(notif, UserId);
            return JsValue.Undefined;
        }
    }
}
