using IridiumJS;
using IridiumJS.Native;
using IridiumJS.Native.Object;
using IridiumJS.Runtime.Interop;

namespace Speercs.Server.Game.Scripting.Api
{
    public class ConsoleAPI : ObjectInstance
    {
        public ConsoleAPI(JSEngine engine) : base(engine)
        {
            FastAddProperty("log", new ClrFunctionInstance(Engine, Log, 0), false, true, false);
            GameAPI.SetDefaultToString(this);
        }
        
        public override string Class
        {
            get { return "Console"; }
        }
        
        private static JsValue Log(JsValue thisObj, JsValue[] args)
        {
            string str = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (i > 0) str += " ";
                str += args[i].ToString();
            }
            System.Console.WriteLine(str);
            return JsValue.Undefined;
        }
    }
}