using LiteDB;
using Newtonsoft.Json;

namespace Speercs.Server.Configuration {
    public class DependencyObject {
        [JsonIgnore]
        [BsonIgnore]
        public ISContext serverContext { get; private set; }

        public DependencyObject(ISContext context) {
            serverContext = context;
        }
    }
}