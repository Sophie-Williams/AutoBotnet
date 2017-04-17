using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api;
using System;

namespace Speercs.Server.Game.Scripting
{
    public class SScriptingHost : DependencyObject
    {
        public SScriptingHost(ISContext context) : base(context)
        {
        }

        public JSEngine CreateSandboxedEngine()
        {
            var engine = new JSEngine(
                cfg =>
                {
                    cfg.LimitRecursion(10);
                    cfg.TimeoutInterval(TimeSpan.FromMilliseconds(ServerContext.Configuration.CodeLoadTimeLimit));
                }
            );
            engine.VariableContext.Game = new SpeercsUserApi();
            return engine;
        }
    }
}