using System;

namespace Speercs.Server.Game.Scripting {
    public class CodeExecutionException : Exception {
        public CodeExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }
}