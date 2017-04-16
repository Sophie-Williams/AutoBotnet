using System.Collections.Generic;

namespace Speercs.Server.Models.Game
{
    /// <summary>
    /// Data model for storing all information about a user and his/her stuff
    /// </summary>
    public class UserTeam
    {
        public List<string> ownedEntities { get; set; }
    }
}
