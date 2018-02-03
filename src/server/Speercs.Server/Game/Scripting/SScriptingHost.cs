using System;
using IridiumJS;
using IridiumJS.Native;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api.Modules;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting {
    public class SScriptingHost : DependencyObject {
        public SScriptingHost(ISContext context) : base(context) { }

        public JSEngine CreateSandboxedEngine(string userId) {
            //-- create the engine
            var engine = new JSEngine(
                cfg => {
                    cfg.LimitRecursion(10);
                    cfg.TimeoutInterval(TimeSpan.FromMilliseconds(ServerContext.Configuration.CodeLoadTimeLimit));
                }
            );

            //--Add modules
            AddGlobal(engine, "Game", new GameApiModule(engine, ServerContext, userId));
//            AddGlobal(engine, "Units", new UnitManagementModule(engine, ServerContext, userId));
//            AddGlobal(engine, "Utils", new UtilsModule(engine, ServerContext, userId));


            // these enum values become numbers (0-3)
            AddGlobal(engine, "NORTH", Direction.North);
            AddGlobal(engine, "EAST", Direction.East);
            AddGlobal(engine, "SOUTH", Direction.South);
            AddGlobal(engine, "WEST", Direction.West);

            //-- return it
            return engine;
        }

        private void AddGlobal(JSEngine engine, string name, object value) {
            engine.Global.FastAddProperty(name, JsValue.FromObject(engine, value), false, true, false);
        }
    }
}