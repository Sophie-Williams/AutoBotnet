using System.Collections.Generic;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Models {
    /// <summary>
    /// Data model for storing all information about a user and his/her stuff
    /// </summary>
    public class UserTeam {
        public string identifier { get; set; }
        public List<GameEntity> ownedEntities { get; set; } = new List<GameEntity>();
        public Dictionary<ResourceId, ulong> resources { get; set; } = new Dictionary<ResourceId, ulong>();
    }
}