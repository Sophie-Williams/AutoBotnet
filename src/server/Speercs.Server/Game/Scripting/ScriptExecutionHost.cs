using System;
using System.Threading;
using IridiumJS;
using IridiumJS.Native;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Application;

namespace Speercs.Server.Game.Scripting {
    public class ScriptExecutionHost {
        public ScriptExecutionHost(JSEngine engine, string userIdentifier, ManualResetEventSlim completionWait) {
            this.engine = engine;
            this.userIdentifier = userIdentifier;
            this.completionWait = completionWait;
        }

        public void execute() {
            try {
                var loopFunc = engine.GetValue("loop");
                engine.ResetTimeoutTicks();
                if (loopFunc != JsValue.Undefined) {
                    result = loopFunc.Invoke();
                }
            } catch (Exception ex) {
                exception = ex;
            }

            completionWait.Set();
        }

        public JsValue result { get; private set; }
        public Exception exception { get; private set; }
        public JSEngine engine { get; }
        public string userIdentifier { get; }
        private ManualResetEventSlim completionWait { get; }
    }
}