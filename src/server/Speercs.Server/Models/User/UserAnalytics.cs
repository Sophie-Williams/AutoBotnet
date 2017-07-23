using Newtonsoft.Json;
using System;

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

        [JsonProperty("apiRequests")]
        public int ApiRequests { get; set; }

        public long LastRequest { get; set; }
    }
}