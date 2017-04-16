using IridiumJS;
using Speercs.Server.Game.Scripting.Api;

namespace Speercs.Server.Game.Scripting
{
    public class SScriptingHost
    {
        public JSEngine CreateSandboxedEngine()
        {
            var engine = new JSEngine(
                cfg => {
                    // TODO: ...
                }
            );
            engine.VariableContext.Game = new SpeercsUserApi();
            return engine;
        }
    }
}