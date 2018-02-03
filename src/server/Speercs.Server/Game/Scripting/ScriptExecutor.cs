using System.Collections.Concurrent;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting {
    public class ScriptExecutor : DependencyObject {
        public ScriptExecutor(JSEngine engine, string userIdentifier, ISContext context) : base(context) {
            this.engine = engine;
            this.userIdentifier = userIdentifier;
        }

        public JSEngine engine { get; }
        public string userIdentifier { get; }
    }
}