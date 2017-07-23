using Newtonsoft.Json;
using System;

namespace Speercs.Server.Models.User
{
    public class UserAnalytics
    {
        [JsonProperty("playtime")]
        public ulong Playtime { get; set; }

        [JsonProperty("codeDeploys")]
        public int CodeDeploys { get; set; }
    
        [JsonProperty("lineCount")]
        public int LineCount { get; set; }

        [JsonProperty("totalLineCount")]
        public int TotalLineCount { get; set; }

        [JsonProperty("apiRequests")]
        public int ApiRequests { get; set; }

        [JsonProperty("lastRequest")]
        public long LastRequest { get; set; }

        [JsonProperty("lastConnection")]
        public long LastConnection { get; set; }
    }
}