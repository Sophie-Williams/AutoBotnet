using System;
using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Entities {
    public enum Direction {
        None = 0,
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    public abstract class GameEntity : ProtectedDependencyObject {
        public string id { get; }

        private RoomPosition _position;

        public RoomPosition position {
            get => _position;
            set { propagatePosition(value); }
        }

        [JsonIgnore]
        public UserTeam team;

        public GameEntity(ISContext serverContext, RoomPosition pos, UserTeam team) : base(serverContext) {
            position = pos;
            this.team = team;

            id = Guid.NewGuid().ToString("N");
            this.serverContext.appState.entities.insert(this);
        }

        private void propagatePosition(RoomPosition pos) {
            // TODO: Propagate position to spatial hash, etc.
            if (serverContext.appState.entities.spatialHash.ContainsKey(pos.roomPos) &&
                serverContext.appState.entities.spatialHash[pos.roomPos].Contains(this)) {
                serverContext.appState.entities.spatialHash[pos.roomPos].Remove(this);
            }

            serverContext.appState.entities.insertSpatialHash(this);
            _position = pos;
        }
    }
}