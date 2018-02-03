using LiteDB;
using Newtonsoft.Json;

namespace Speercs.Server.Configuration {
    public class ProtectedDependencyObject {
        [JsonIgnore]
        [BsonIgnore]
        protected ISContext ServerContext { get; private set; }

        public ProtectedDependencyObject(ISContext context) {
            ServerContext = context;
        }
    }
}