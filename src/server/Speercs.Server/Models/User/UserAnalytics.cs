using Newtonsoft.Json;
using System;

namespace Speercs.Server.Models.User {
    public class UserAnalytics {
        [JsonProperty("playtime")]
        public ulong playtime { get; set; }

        [JsonProperty("codeDeploys")]
        public ulong codeDeploys { get; set; }

        [JsonProperty("lineCount")]
        public ulong lineCount { get; set; }

        [JsonProperty("totalLineCount")]
        public ulong totalLineCount { get; set; }

        [JsonProperty("apiRequests")]
        public ulong apiRequests { get; set; }

        [JsonProperty("lastRequest")]
        public ulong lastRequest { get; set; }

        [JsonProperty("lastConnection")]
        public ulong lastConnection { get; set; }
    }
}