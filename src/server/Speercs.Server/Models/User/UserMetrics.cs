using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Newtonsoft.Json;

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
        public ulong playtime { get; set; }

        [BsonIgnore]
        public int codeDeploys => events.Where(x => x.type == MetricsEventType.CodeDeploy).Count();

        public ulong lineCount { get; set; }

        public ulong totalLineCount { get; set; }

        public ulong lastRealtimeCollection { get; set; }

        public List<MetricsEvent> events { get; set; } = new List<MetricsEvent>();
    }
}