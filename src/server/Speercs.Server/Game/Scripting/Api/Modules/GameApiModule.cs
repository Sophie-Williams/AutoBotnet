using System;
using IridiumJS;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class GameApiModule : ScriptingApiModule {
        public GameApiModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            bool push(object obj) {
                var pushMessageTask = context.notificationPipeline.pushMessageAsync(JToken.FromObject(obj),
                        userId, true);
                return true;
            }

            defineFunction("getUserIdentifier", () => this.userId);
            defineFunction("push", new Func<object, bool>(push));
        }
    }
}