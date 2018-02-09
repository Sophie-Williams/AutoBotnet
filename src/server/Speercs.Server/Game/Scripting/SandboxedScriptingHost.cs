using System;
using IridiumJS;
using IridiumJS.Native;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api.Modules;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting {
    public class SandboxedScriptingHost : DependencyObject {
        public SandboxedScriptingHost(ISContext context) : base(context) { }

        public JSEngine createSandboxedEngine(string userId) {
            // create the engine
            var engine = new JSEngine(
                cfg => {
                    cfg.LimitRecursion(10);
                    cfg.TimeoutInterval(TimeSpan.FromMilliseconds(serverContext.configuration.codeLoadTimeLimit));
                }
            );

            // Add userapi module globals
            addGlobal(engine, "Game", new GameApiModule(engine, serverContext, userId));
            addGlobal(engine, "Utils", new UtilsModule(engine, serverContext, userId));
            addGlobal(engine, "Army", new ArmyModule(engine, serverContext, userId));


            // these enum values become numbers (0-3)
            addGlobal(engine, "NORTH", Direction.North);
            addGlobal(engine, "EAST", Direction.East);
            addGlobal(engine, "SOUTH", Direction.South);
            addGlobal(engine, "WEST", Direction.West);

            return engine;
        }

        private void addGlobal(JSEngine engine, string name, object value) {
            engine.Global.FastAddProperty(name, JsValue.FromObject(engine, value), false, true, false);
        }
    }
}