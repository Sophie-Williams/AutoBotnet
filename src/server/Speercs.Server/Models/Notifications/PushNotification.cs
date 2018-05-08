using System;
using Newtonsoft.Json;

namespace Speercs.Server.Models.Notifications {
    public class PushNotification {
        public long time { get; set; }

        public string type { get; set; }

        public object content { get; set; }
        
        public string source { get; set; }

        public PushNotification(string source, string type, object content) {
            this.source = source;
            this.type = type;
            this.content = content;
            time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}