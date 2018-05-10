using System;
using IridiumJS;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Notifications;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class GameApiModule : ScriptingApiModule {
        public GameApiModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            bool push(object data, string type) {
                var pushMessageTask = context.notificationPipeline.pushMessageAsync(JToken.FromObject(
                    new PushNotification("user", type, data)
                ), "user", userId, true);
                return true;
            }

            defineFunction("getId", () => this.userId);
            defineFunction("push", new Func<object, string, bool>(push));
        }
    }
}