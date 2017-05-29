using System.Collections.Generic;
using Speercs.Server.Models.User;

namespace Speercs.Server.Models.Game
{
    /// <summary>
    /// Data model for storing all information about a user and his/her stuff
    /// </summary>
    public class UserTeam
    {
        public RegisteredUser User { get; }
        
        public List<string> OwnedEntities { get; set; }
    }
}
