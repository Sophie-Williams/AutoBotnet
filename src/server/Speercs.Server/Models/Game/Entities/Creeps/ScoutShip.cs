using Speercs.Server.Configuration;

namespace Speercs.Server.Models.Game.Entities.Creeps
{
    public class ScoutShip : CreepShip
    {
        public ScoutShip(SContext serverContext, string startRoom)
        {
            ServerContext = serverContext;
            RoomIdentifier = startRoom;
        }
    }
}
