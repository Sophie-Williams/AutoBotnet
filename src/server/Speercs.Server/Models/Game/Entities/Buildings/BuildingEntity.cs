using System;
using System.Collections.Generic;
using LiteDB;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities.Buildings {
    public abstract class BuildingEntity : GameEntity {
        /// <summary>
        /// Bson constructor
        /// </summary>
        public BuildingEntity() { }

        protected BuildingEntity(RoomPosition pos, UserEmpire team) :
            base(pos, team) { }
        
        [BsonIgnore]
        public Dictionary<string, Delegate> actions { get; } = new Dictionary<string, Delegate>();
        
        public void defineAction(string name, Delegate func) {
            actions[name] = func;
        }
    }
}