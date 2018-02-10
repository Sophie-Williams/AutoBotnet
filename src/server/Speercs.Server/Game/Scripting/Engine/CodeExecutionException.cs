using System;

namespace Speercs.Server.Game.Scripting.Engine {
    public class CodeExecutionException : Exception {
        public CodeExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }
}