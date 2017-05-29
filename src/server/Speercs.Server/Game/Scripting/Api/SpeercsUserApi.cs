using System.Collections.Generic;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility;
using Speercs.Server.Models.Game;
using Speercs.Server.Models.Game.Entities;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.Scripting.Api
{
    public class SpeercsUserApi : ProtectedDependencyObject
    {
        public string UserId { get; }
        private UserTeam MyTeam { get; set; }
        // Placeholder untill these are in proper classes
        public int CurrentRoomX { get; set; } = 0;
        public int CurrentRoomY { get; set; } = 0;
        public int NRG
        {
            get
            {
                return MyTeam.NRG;
            }
        }

        public SpeercsUserApi(ISContext serverContext, string userId) : base(serverContext)
        {
            UserId = userId;
            MyTeam = ServerContext.AppState.PlayerData[UserId];
        }

        public List<GameEntity> Entities
        {
            get
            {
                return ServerContext.AppState.Entities.GetAllByUser(ServerContext.AppState.PlayerData[UserId]);
            }
        }

        public Room CurrentRoom
        {
            get
            {
                return ServerContext.AppState.WorldMap[CurrentRoomX, CurrentRoomY];
            }
        }

        public ITile GetTile(int x, int y)
        {
            return CurrentRoom.Tiles[x, y];
        }

        public bool MineTile(int x, int y)
        {
            // TODO: Make sure entity is near tile
            ITile tile = CurrentRoom.Tiles[x, y];
            if (tile.IsMinable())
            {
                // TODO: Mine tile
                return true;
            }
            return false;
        }

        public Room MoveToRoom(int x, int y)
        {
            Room newRoom = ServerContext.AppState.WorldMap[x, y];
            if (newRoom == null) return null;
            (CurrentRoomX, CurrentRoomY) = (x, y);
            return newRoom;
        }

        // To run after modifying MyTeam
        private void UpdateData()
        {
            ServerContext.AppState.PlayerData[UserId] = MyTeam;
        }

        public bool SpawnEntity()
        {
            // TODO: Stuff
            return false;
        }
    }
}