using System;
using Newtonsoft.Json;

namespace Speercs.Server.Models.Game.Program {
    public class UserProgram {
        [JsonProperty("deployTime")]
        public long deployTime { get; set; }

        [JsonProperty("source")]
        public string source { get; set; }

        public UserProgram(string programSource) {
            source = programSource;
            deployTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        // BSON Property
        public UserProgram() { }
    }
}