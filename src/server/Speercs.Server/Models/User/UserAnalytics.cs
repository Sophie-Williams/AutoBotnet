using Newtonsoft.Json;
using System;

namespace Speercs.Server.Models.User {
    public class UserAnalytics {
        [JsonProperty("playtime")]
        public ulong Playtime { get; set; }

        [JsonProperty("codeDeploys")]
        public ulong CodeDeploys { get; set; }

        [JsonProperty("lineCount")]
        public ulong LineCount { get; set; }

        [JsonProperty("totalLineCount")]
        public ulong TotalLineCount { get; set; }

        [JsonProperty("apiRequests")]
        public ulong ApiRequests { get; set; }

        [JsonProperty("lastRequest")]
        public ulong LastRequest { get; set; }

        [JsonProperty("lastConnection")]
        public ulong LastConnection { get; set; }
    }
}