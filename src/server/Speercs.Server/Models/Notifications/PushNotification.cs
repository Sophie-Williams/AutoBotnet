using System;
using Newtonsoft.Json;

namespace Speercs.Server.Models.Notifications {
    public class PushNotification {
        [JsonProperty("notifTime")]
        public long notifTime { get; set; }

        [JsonProperty("type")]
        public string notifType { get; set; }

        [JsonProperty("content")]
        public string notifContent { get; set; }

        public PushNotification(string type, string contents) {
            notifType = type;
            notifContent = contents;
            notifTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}