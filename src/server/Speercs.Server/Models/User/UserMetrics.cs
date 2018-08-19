using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace Speercs.Server.Models.User {
    public enum MetricsEventType {
        Unspecified,
        Auth,
        CodeDeploy,
        ApiRequest,
        RealtimeConnect,
    }
    public enum MetricsLevel {
        Minimal,
        Full
    }
    public class MetricsEvent {
        public MetricsEventType type { get; set; }
        public long time { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    public class UserMetrics : DatabaseObject {
        public string userId;

        public ulong playtime;

        [BsonIgnore]
        public int codeDeploys => events.Count(x => x.type == MetricsEventType.CodeDeploy);

        public ulong lineCount;

        public ulong totalLineCount;

        public ulong lastRealtimeConnection;

        public List<MetricsEvent> events = new List<MetricsEvent>();
    }
}