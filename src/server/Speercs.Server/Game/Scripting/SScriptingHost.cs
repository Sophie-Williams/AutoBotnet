using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api;
using System;
using IridiumJS.Native;

namespace Speercs.Server.Game.Scripting
{
    public class SScriptingHost : DependencyObject
    {
        public SScriptingHost(ISContext context) : base(context)
        {
        }

        public JSEngine CreateSandboxedEngine(string userId)
        {
            var engine = new JSEngine(
                cfg =>
                {
                    cfg.LimitRecursion(10);
                    cfg.TimeoutInterval(TimeSpan.FromMilliseconds(ServerContext.Configuration.CodeLoadTimeLimit));
                }
            );
            var gameObj = JsValue.FromObject(engine, new SpeercsUserApi(ServerContext, userId));
            engine.Global.FastAddProperty("Game", gameObj, false, true, false);
            return engine;
        }
    }
}