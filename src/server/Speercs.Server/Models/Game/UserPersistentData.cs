using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Speercs.Server.Models.Program;

namespace Speercs.Server.Models {
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

        public UserEmpire team { get; set; } = new UserEmpire();
    }
}