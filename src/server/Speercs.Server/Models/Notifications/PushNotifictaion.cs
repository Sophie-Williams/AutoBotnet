using Newtonsoft.Json;
using System;

namespace Speercs.Server.Models.Notifications {
    public class PushNotification {
        [JsonProperty("notifTime")]
        public DateTime NotifTime { get; set; }

        [JsonProperty("type")]
        public string NotifType { get; set; }

        [JsonProperty("content")]
        public string NotifContent { get; set; }

        public PushNotification(string type, string contents) {
            NotifType = type;
            NotifContent = contents;
            NotifTime = DateTime.Now;
        }
    }
}