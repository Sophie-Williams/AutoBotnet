using LiteDB;
using Newtonsoft.Json;

namespace Speercs.Server.Configuration {
    public class ProtectedDependencyObject {
        [JsonIgnore]
        [BsonIgnore]
        protected ISContext serverContext { get; private set; }

        public ProtectedDependencyObject(ISContext context) {
            serverContext = context;
        }
    }
}