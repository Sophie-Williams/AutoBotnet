using System;
using LiteDB;
using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;
using Speercs.Server.Services.EventPush;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Models.Entities {
    public enum Direction {
        None = 0,
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    public abstract class GameEntity : DatabaseObject {
        public const string EVENT_POSITION = "pos";
        public const string EVENT_DIE = "die";

        [BsonField("eid")]
        public string id { get; set; }

        [BsonField("health")]
        public int health { get; set; } = 0;

        [BsonField("maxhealth")]
        public int maxhealth { get; set; } = 1;

        private RoomPosition _position { get; set; }

        [BsonIgnore] [JsonIgnore] public ISContext context;

        [BsonField("position")]
        public RoomPosition position {
            get => _position;
            set { propagatePosition(value); }
        }

        [JsonIgnore]
        [BsonField("teamId")]
        public string teamId { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public UserEmpire team { get; private set; }

        [BsonIgnore]
        public string type => this.GetType().Name;

        /// <summary>
        /// Bson constructor
        /// </summary>
        public GameEntity() { }

        public GameEntity(RoomPosition pos, UserEmpire team) {
            position = pos;
            teamId = team.identifier;
            id = Guid.NewGuid().ToString("N");
        }

        public void resetHealth(int initialHealth) {
            maxhealth = health = initialHealth;
        }

        public void loadContext(ISContext context) {
            this.context = context;
            var userDataService = new PersistentDataService(context);
            team = userDataService.get(teamId).team;
            propagatePosition(_position);
        }

        public virtual void wake() { } // called on unpickling

        /// <summary>
        /// lifecycle tick
        /// </summary>
        public virtual bool tick() {
            // TODO: health/integrity check (check cores)
            return true;
        }

        public override bool Equals(object obj) {
            return obj is GameEntity ge && this.id == ge.id;
        }

        public override int GetHashCode() => id.GetHashCode();

        private bool propagatePosition(RoomPosition pos) {
            _position = pos;
            if (context == null) return false;
            // Propagate position to spatial hash, etc.
            if (context.appState.entities.spatialHash.ContainsKey(pos.roomPos.ToString()) &&
                context.appState.entities.spatialHash[pos.roomPos.ToString()].Contains(this)) {
                context.appState.entities.spatialHash[pos.roomPos.ToString()].Remove(this);
            }

            context.appState.entities.insertSpatialHash(this);
            raiseEvent(EVENT_POSITION, pos);
            return true;
        }

        public (bool, bool) raiseEvent(string type, object data) {
            if (context == null) return (false, false);
            var result = context.eventPush.pushEvent(EventPushService.EVENTPUSH_ENTITY, type, this, data);
            return (true, result);
        }
    }
}