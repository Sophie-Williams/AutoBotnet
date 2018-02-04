using System;
using Newtonsoft.Json;

namespace Speercs.Server.Models.Game.Program {
    public class UserProgram {
        [JsonProperty("deployTime")]
        public DateTime deployTime { get; set; }

        [JsonProperty("source")]
        public string source { get; set; } = "\nfunction loop () {\n  // your code\n}\n";

        public UserProgram(string programSource) {
            source = programSource;
            deployTime = DateTime.Now;
        }

        // BSON Property
        public UserProgram() { }
    }
}