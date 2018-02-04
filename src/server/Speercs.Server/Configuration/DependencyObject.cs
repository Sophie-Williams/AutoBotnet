using LiteDB;
using Newtonsoft.Json;

namespace Speercs.Server.Configuration {
    public class DependencyObject {
        [JsonIgnore]
        [BsonIgnore]
        protected ISContext serverContext { get; private set; }

        protected DependencyObject(ISContext context) {
            serverContext = context;
        }
    }
}