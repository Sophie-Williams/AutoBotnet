using System.Collections.Generic;
using IridiumJS;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class BotCoreRef : GameObjectRef {
        private BotCore _core;

        public BotCoreRef(JSEngine engine, BotCore core) : base(engine) {
            _core = core;
        }

        public Dictionary<string, long> qualities => _core.qualities;
        public int drain => _core.drain;
        public BotCoreFlags flags => _core.flags;
        public int size => _core.drain;
    }
}