using LiteDB;
using System;
using Speercs.Server.Models;

namespace Speercs.Server.Configuration
{
    /// <summary>
    /// Persisted state for the server
    /// </summary>
    public class SAppState : DatabaseObject
    {
        [BsonIgnore]
        public LiteCollection<SAppState> PersistenceMedium { get; set; }

        [BsonIgnore]
        public Action Persist { get; set; }
    }
}