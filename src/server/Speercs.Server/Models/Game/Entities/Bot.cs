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
            
            lastMove = ServerContext.AppState.TickCount;
            return base.MoveRelative(direction);
        }
        
        protected long lastMove = long.MinValue;
        
        public UserTeam Team { get; }
    }
}
