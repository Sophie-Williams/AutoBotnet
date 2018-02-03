using LiteDB;
using Newtonsoft.Json;

namespace Speercs.Server.Configuration {
    public class DependencyObject {
        [JsonIgnore]
        [BsonIgnore]
        public ISContext ServerContext { get; private set; }

        public DependencyObject(ISContext context) {
            ServerContext = context;
        }
    }
}