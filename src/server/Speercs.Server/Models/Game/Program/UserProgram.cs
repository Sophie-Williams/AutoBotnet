using Newtonsoft.Json;
using System;

namespace Speercs.Server.Models.Game.Program
{
    public class UserProgram
    {
        [JsonProperty("deployTime")]
        public DateTime DeployTime { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        public UserProgram(string programSource)
        {
            Source = programSource;
            DeployTime = DateTime.Now;
        }

        // BSON Property
        public UserProgram()
        {

        }
    }
}
