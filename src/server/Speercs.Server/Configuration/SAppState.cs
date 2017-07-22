using LiteDB;
using Speercs.Server.Models;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Game.Map;
using System;
using System.Collections.Generic;

namespace Speercs.Server.Configuration
{
    /// <summary>
    /// Persisted state for the server
    /// </summary>
    public class SAppState : DatabaseObject
    {
        [BsonIgnore]
        public LiteCollection<SAppState> PersistenceMedium { get; set; }

        /// <summary>
        /// Persist the data container to disk. Call QueuePersist() if requesting a persist.
        /// </summary>
        /// <returns></returns>
        [BsonIgnore]
        public Action<bool> Persist { get; set; }
        

        [BsonIgnore]
        public bool PersistNeeded { get; set; }

        [BsonIgnore]
        public bool PersistAvailable { get; set; } = true;

        /// <summary>
        /// Call this to queue a persist.
        /// </summary>
        public void QueuePersist()
        {
            PersistNeeded = true;
        }

        public Dictionary<string, UserTeam> PlayerData { get; set; } = new Dictionary<string, UserTeam>();

        public List<string> InviteKeys { get; set; } = new List<string>();

        public WorldMap WorldMap { get; set; } = new WorldMap();

        public EntityBag Entities { get; set; } = new EntityBag();
        
        public ulong TickCount { get; set; }
    }
}
