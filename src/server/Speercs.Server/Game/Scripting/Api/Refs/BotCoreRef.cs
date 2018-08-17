using System;
using System.Collections.Generic;
using System.Diagnostics;
using Speercs.Server.Models.Entities;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class BotCoreRef : GameObjectRef {
        private BotCore _core;

        public BotCoreRef(BotCore core) {
            _core = core;
        }

        public object call(string name, params object[] args) {
            if (_core.actions.ContainsKey(name)) {
                try {
                    return _core.actions[name].DynamicInvoke(args);
                } catch (Exception callException) {
                    Trace.WriteLine(callException);
                }
            }
            return null;
        }

        public string type => _core.type;
        public Dictionary<string, long> qualities => _core.qualities;
        public int drain => _core.drain;
        public BotCoreFlags flags => _core.flags;
        public int size => _core.drain;

        public void destroy() {
            _core = null;
        }
    }
}