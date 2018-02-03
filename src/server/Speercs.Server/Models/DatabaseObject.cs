using LiteDB;
using Newtonsoft.Json;

namespace Speercs.Server.Models {
    public class DatabaseObject {
        [JsonIgnore]
        [BsonId]
        public ObjectId databaseId { get; set; }
    }
}