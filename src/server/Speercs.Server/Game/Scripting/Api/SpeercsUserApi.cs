using System.Collections.Generic;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Entities;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class SpeercsUserApi : ProtectedDependencyObject
    {
        public string UserId { get; }
        public int CurrentRoomX { get; set; } = 0;
        public int CurrentRoomY { get; set; } = 0;

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
            return ServerContext.AppState.WorldMap[CurrentRoomX,CurrentRoomY];
        }

        public Room MoveToRoom(int x, int y) {
            Room newRoom = ServerContext.AppState.WorldMap[x,y];
            if (newRoom == null) return null;
            (CurrentRoomX, CurrentRoomY) = (x,y);
            return newRoom;
        }

        public string Test() {
            return "This is a test.";
        }
    }
}