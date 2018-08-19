using System;
using System.Collections.Generic;
using LiteDB;
using Speercs.Server.Models;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.User;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Speercs.Server.Configuration {
    /// <summary>
    /// Persisted state for the server
    /// </summary>
    public class SAppState : DatabaseObject {
        [BsonIgnore]
        public LiteCollection<SAppState> persistenceMedium { get; set; }

        /// <summary>
        /// Persist the data container to disk. Call QueuePersist() if requesting a persist.
        /// </summary>
        /// <returns></returns>
        [BsonIgnore]
        public Action<bool> persist { get; set; }

        [BsonIgnore]
        public bool persistNeeded { get; set; }

        [BsonIgnore]
        public bool persistAvailable { get; set; } = true;

        /// <summary>
        /// Call this to queue a persist.
        /// </summary>
        public void queuePersist() {
            persistNeeded = true;
        }
        
        public WorldMap worldMap { get; set; } = new WorldMap();

        public EntityBag entities { get; set; } = new EntityBag();
        
        public Dictionary<string, UserPersistentData> userPersistentData { get; set; } = new Dictionary<string, UserPersistentData>();

        public ulong tickCount { get; set; }
    }
}