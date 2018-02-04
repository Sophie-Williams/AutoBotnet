using System;

namespace Speercs.Server.Services.Application {
    public class SpeercsLogger {
        public SpeercsLogger(LogLevel verbosity) {
            this.verbosity = verbosity;
        }
        
        public void writeLine(string log, LogLevel level) {
            if (level >= verbosity) {
                Console.WriteLine($"[{level.ToString()}] {log}");
            }
        }

        public LogLevel verbosity = LogLevel.Warning;

        public enum LogLevel {
            Trace,
            Information,
            Warning,
            Error,
            Critical
        }
    }
}