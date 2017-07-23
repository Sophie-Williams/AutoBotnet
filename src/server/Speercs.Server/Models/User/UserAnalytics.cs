using Newtonsoft.Json;

namespace Speercs.Server.Models.User
{
    public class UserAnalytics
    {
        [JsonProperty("playtime")]
        public int Playtime { get; set; }

        [JsonProperty("codeDeploys")]
        public int CodeDeploys { get; set; }
    
        [JsonProperty("lineCount")]
        public int LineCount { get; set; }
    }
}