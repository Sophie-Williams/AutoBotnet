using System;
using LiteDB;
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

    public abstract class GameEntity {
        [BsonId]
        public string id { get; }

        private RoomPosition _position;

        protected ISContext _context;

        [BsonField("position")]
        public RoomPosition position {
            get => _position;
            set { propagatePosition(value); }
        }

        [JsonIgnore] [BsonField("team")] public UserTeam team;

        /// <summary>
        /// Bson constructor
        /// </summary>
        public GameEntity() { }

        public GameEntity(RoomPosition pos, UserTeam team) {
            position = pos;
            this.team = team;

            id = Guid.NewGuid().ToString("N");
        }

        internal void loadContext(ISContext context) {
            _context = context;
        }

        public virtual void wake() { }

        private bool propagatePosition(RoomPosition pos) {
            _position = pos;
            if (_context == null) return false;
            // Propagate position to spatial hash, etc.
            if (_context.appState.entities.spatialHash.ContainsKey(pos.roomPos.ToString()) &&
                _context.appState.entities.spatialHash[pos.roomPos.ToString()].Contains(this)) {
                _context.appState.entities.spatialHash[pos.roomPos.ToString()].Remove(this);
            }

            _context.appState.entities.insertSpatialHash(this);
            return true;
        }
    }
}