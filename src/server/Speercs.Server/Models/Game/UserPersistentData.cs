using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Speercs.Server.Models.Game.Program;

namespace Speercs.Server.Models.Game {
    public class UserPersistentData : DatabaseObject {
        public UserPersistentData(string ownerId) {
            this.ownerId = ownerId;
        }

        // BSON Constructor
        // ReSharper disable once UnusedMember.Global
        public UserPersistentData() { }

        public string ownerId { get; set; }

        public UserProgram program { get; set; }

        public Queue<JToken> queuedNotifications { get; set; } = new Queue<JToken>();

        public UserTeam team { get; set; } = new UserTeam();
    }
}