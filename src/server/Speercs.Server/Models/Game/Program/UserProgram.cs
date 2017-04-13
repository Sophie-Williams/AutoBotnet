using Newtonsoft.Json;
using System;

namespace Speercs.Server.Models.Game.Program
{
    public class UserProgram
    {
        [JsonProperty("deployTime")]
        public DateTime DeployTime { get; }

        [JsonProperty("source")]
        public string Source { get; }

        public UserProgram(string programSource)
        {
            Source = programSource;
        }
    }
}
