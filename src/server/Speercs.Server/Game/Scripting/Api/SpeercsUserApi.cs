using System.Collections.Generic;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Entities;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class SpeercsUserApi : ProtectedDependencyObject
    {
        public string UserId { get; }
        public (int, int) RoomLocation { get; set; } = (0,0);

        public SpeercsUserApi(ISContext serverContext, string userId) : base(serverContext)
        {
            UserId = userId;
        }

        /*public List<GameEntity> Entities
        {
            get
            {
                return ServerContext.AppState.Entities.GetAllByUser(ServerContext.AppState.PlayerData[UserId]);
            }
        }*/

        public Room GetCurrentRoom() {
            var (roomX, roomY) = RoomLocation;
            return ServerContext.AppState.WorldMap[roomX,roomY];
        }
    }
}