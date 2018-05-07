using System;
using System.Collections.Generic;
using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Descriptors;
using IridiumJS.Runtime.Interop;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Api {
    public class ScriptingApiModule : ScriptObjectInstance {
        protected readonly ISContext context;
        protected readonly string userId;

        public ScriptingApiModule(JSEngine engine, ISContext context, string userId) : base(engine) {
            this.context = context;
            this.userId = userId;
            var persistentData = new PersistentDataService(context).get(userId);
            setDefaultToString();
        }
    }
}