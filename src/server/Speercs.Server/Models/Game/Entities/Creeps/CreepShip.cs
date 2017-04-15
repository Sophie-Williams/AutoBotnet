using Speercs.Server.Configuration;

namespace Speercs.Server.Models.Game.Entities.Creeps
{
    public abstract class CreepShip : GameEntity
    {
        public CreepShip(ISContext serverContext) : base(serverContext)
        {
        }
    }
}
