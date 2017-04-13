using Speercs.Server.Models.Game.Program;

namespace Speercs.Server.Models.Game
{
    public class UserPersistentData
    {
        public string OwnerId { get; }
        public UserProgram Program { get; set; }
    }
}
