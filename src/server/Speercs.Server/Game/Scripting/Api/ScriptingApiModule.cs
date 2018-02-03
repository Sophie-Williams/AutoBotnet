using System;
using System.Collections.Generic;
using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Descriptors;
using IridiumJS.Runtime.Interop;
using Speercs.Server.Configuration;

namespace Speercs.Server.Game.Scripting.Api {
    public class ScriptingApiModule : ObjectInstance {
        protected readonly ISContext _context;
        protected readonly string _userId;
        protected List<string> myEntities;

        public ScriptingApiModule(JSEngine engine, ISContext context, string userId) : base(engine) {
            _context = context;
            _userId = userId;
            myEntities = context.AppState.PlayerData[userId].OwnedEntities;
            SetDefaultToString();
        }

        protected PropertyDescriptor MakeFunctionProperty(JSEngine engine, Delegate func) {
            return new PropertyDescriptor(new DelegateWrapper(engine, func), false, false, false);
        }

        protected void DefineFunction<T>(string name, Func<T> func) {
            FastSetProperty(name, MakeFunctionProperty(Engine, func));
        }

        protected void DefineFunction(string name, Delegate func) {
            FastSetProperty(name, MakeFunctionProperty(Engine, func));
        }

        protected void AddGlobal(string name, object value) {
            Engine.Global.FastAddProperty(name, JsValue.FromObject(Engine, value), false, true, false);
        }

        private void SetDefaultToString() {
            DefineFunction("toString", () => $"[object {Class}]");
        }
    }
}