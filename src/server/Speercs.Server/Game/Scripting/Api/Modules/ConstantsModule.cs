using System;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Entities;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ConstantsModule : ScriptingApiModule {
        public ConstantsModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            defineEnum<Direction>();
            defineEnum<BotTemplates>(true);
        }

        private void defineEnum<T>(bool prefix = false) where T : struct {
            var names = Enum.GetNames(typeof(T));
            foreach (var name in names) {
                var propertyName = prefix ? $"{typeof(T).Name.ToUpper()}_{name.ToUpper()}" : name.ToUpper();
                addReadOnlyProperty(propertyName, (int) Enum.Parse(typeof(T), name));
            }
        }
    }
}