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
    public class ScriptObjectInstance : ObjectInstance {
        public ScriptObjectInstance(JSEngine engine) : base(engine) {
            defineFunction("toString", () => $"[object {this.GetType().Name}]");
        }

        protected void defineFunction<T>(string name, Func<T> func) {
            FastSetProperty(name, makeFunctionProperty(Engine, func));
        }

        protected void defineFunction(string name, Delegate func) {
            FastSetProperty(name, makeFunctionProperty(Engine, func));
        }

        protected PropertyDescriptor makeProperty(object value, bool writable, bool enumerable,
            bool configurable = false) {
            return new PropertyDescriptor(JsValue.FromObject(Engine, value), writable, enumerable, configurable);
        }

        protected PropertyDescriptor makeFunctionProperty(JSEngine engine, Delegate func) {
            return new PropertyDescriptor(new DelegateWrapper(engine, func), false, false, false);
        }
        protected void addGlobal(string name, object value) {
            Engine.Global.FastAddProperty(name, JsValue.FromObject(Engine, value), false, true, false);
        }

        protected void addReadOnlyProperty(string name, object value) {
            FastSetProperty(name, makeProperty(value, false, true));
        }

        protected void setDefaultToString() {
            defineFunction("toString", () => $"[object {Class}]");
        }
    }
}