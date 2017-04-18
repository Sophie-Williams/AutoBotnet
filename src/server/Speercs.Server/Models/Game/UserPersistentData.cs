using Speercs.Server.Models.Game.Program;

namespace Speercs.Server.Models.Game
{
    public class UserPersistentData : DatabaseObject
    {
        public UserPersistentData(string ownerId)
        {
            OwnerId = ownerId;
        }

        public string OwnerId { get; }

        public UserProgram Program { get; set; }
    }
}
