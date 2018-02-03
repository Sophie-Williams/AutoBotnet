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
        protected readonly ISContext context;
        protected readonly string userId;
        protected List<string> myEntities;

        public ScriptingApiModule(JSEngine engine, ISContext context, string userId) : base(engine) {
            this.context = context;
            this.userId = userId;
            myEntities = context.appState.playerData[userId].ownedEntities;
            setDefaultToString();
        }

        protected PropertyDescriptor makeFunctionProperty(JSEngine engine, Delegate func) {
            return new PropertyDescriptor(new DelegateWrapper(engine, func), false, false, false);
        }

        protected void defineFunction<T>(string name, Func<T> func) {
            FastSetProperty(name, makeFunctionProperty(Engine, func));
        }

        protected void defineFunction(string name, Delegate func) {
            FastSetProperty(name, makeFunctionProperty(Engine, func));
        }

        protected void addGlobal(string name, object value) {
            Engine.Global.FastAddProperty(name, JsValue.FromObject(Engine, value), false, true, false);
        }

        private void setDefaultToString() {
            defineFunction("toString", () => $"[object {Class}]");
        }
    }
}