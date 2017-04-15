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

        [BsonIgnore]
        public Action Persist { get; set; }

        public Dictionary<string, UserTeam> PlayerData { get; set; } = new Dictionary<string, UserTeam>();

        public WorldMap WorldMap { get; set; } = new WorldMap();

        public EntityBag Entities { get; set; } = new EntityBag();
    }
}
