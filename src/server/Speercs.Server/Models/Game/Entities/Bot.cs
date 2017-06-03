using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Models.Game.Entities
{
    public class Bot : GameEntity
    {
        public Bot(ISContext serverContext, RoomPosition pos, UserTeam team) : base(serverContext, pos)
        {
            Team = team;
        }
        
        public override bool MoveRelative(Direction direction)
        {
            if (ServerContext.AppState.TickCount <= lastMove)
                return false; // already moved this tick
            
            if (base.MoveRelative(direction))
            {
                lastMove = ServerContext.AppState.TickCount;
                return true;
            }
            else return false;
        }
        
        protected long lastMove = long.MinValue;
        
        public UserTeam Team { get; }
    }
}
