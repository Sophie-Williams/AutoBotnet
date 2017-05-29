using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Models.Game.Entities
{
    public class Bot : GameEntity
    {
        public Bot(ISContext serverContext, RoomPosition pos) : base(serverContext, pos)
        {
        }
    }
}
