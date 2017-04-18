using System.Collections.Generic;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Entities;

namespace Speercs.Server.Game.Scripting.Api
{
    public class SpeercsUserApi
    {
        public string UserId { get; }
        public string RoomLocation { get; set; }
        private ISContext ServerContext { get; set; }

        public SpeercsUserApi(string userId, ISContext serverContext)
        {
            UserId = userId;
            ServerContext = serverContext;
        }

        public List<GameEntity> Entities
        {
            get
            {
                return ServerContext.AppState.Entities.GetAllByUser(ServerContext.AppState.PlayerData[UserId]);
            }
        }
    }
}