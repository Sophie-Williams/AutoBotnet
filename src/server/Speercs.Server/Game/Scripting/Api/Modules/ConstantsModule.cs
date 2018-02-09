using System;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Entities;
using Speercs.Server.Models.Game.Materials;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ConstantsModule : ScriptingApiModule {
        public ConstantsModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            
            defineEnum<Direction>();
            defineEnum<BotTemplates>();
        }

        private void defineEnum<T>() where T : struct {
            var names = Enum.GetNames(typeof(T));
            foreach (var name in names) {
                addGlobal(name.ToUpper(), (int)Enum.Parse(typeof(T), name));
            }
        }
    }
}