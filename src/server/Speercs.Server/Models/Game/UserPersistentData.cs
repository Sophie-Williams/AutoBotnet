using Speercs.Server.Models.Game.Program;

namespace Speercs.Server.Models.Game
{
    public class UserPersistentData : DatabaseObject
    {
        public UserPersistentData(string ownerId)
        {
            OwnerId = ownerId;
        }

        // BSON Constructor
        public UserPersistentData()
        {
        }

        public string OwnerId { get; set; }

        public UserProgram Program { get; set; }
    }
}
