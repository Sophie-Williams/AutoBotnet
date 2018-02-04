using IridiumJS;
using Speercs.Server.Configuration;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class GameApiModule : ScriptingApiModule {
        public GameApiModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            defineFunction("getUserIdentifier", () => this.userId);
        }
    }
}