using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Interop;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Speercs.Server.Game.Scripting.Api
{
    public class ConsoleApi : ObjectInstance
    {
        private ISContext ServerContext { get; set; }
        private string UserId { get; set; }
        public ConsoleApi(JSEngine engine, ISContext serverContext, string userId) : base(engine)
        {
            ServerContext = serverContext;
            UserId = userId;
            FastAddProperty("log", new ClrFunctionInstance(Engine, Log, 0), false, true, false);
            FastAddProperty("notify", new ClrFunctionInstance(Engine, Notify, 0), false, true, false);
            GameApi.SetDefaultToString(this);
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
            // Queue the push notification
            Task.Run(() => ServerContext.NotificationPipeline.PushMessageAsync(notif, UserId));
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
            Task.Run(() => ServerContext.NotificationPipeline.PushMessageAsync(notif, UserId));
            return JsValue.Undefined;
        }
    }
}
