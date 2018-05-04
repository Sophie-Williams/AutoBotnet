using System.Collections.Generic;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Models {
    /// <summary>
    /// Data model for storing all information about a user and his/her stuff
    /// </summary>
    public class UserTeam {
        public string identifier { get; set; }
        public List<string> ownedEntities { get; set; } = new List<string>();
        public Dictionary<ResourceId, ulong> resources { get; set; } = new Dictionary<ResourceId, ulong>();
    }
}