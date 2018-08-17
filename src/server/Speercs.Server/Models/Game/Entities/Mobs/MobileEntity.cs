using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Entities {
    public abstract class MobileEntity : GameEntity {
        /// <summary>
        /// BsonConstructor
        /// </summary>
        public MobileEntity() { }

        protected MobileEntity(RoomPosition pos, UserTeam team) : base(pos, team) { }

        public bool move(Direction direction) {
            return moveRelative(direction);
        }

        protected virtual bool moveRelative(Direction direction) {
            var newPos = position.move(direction);
            
            // if the new position is a different room, ensure we can move there
            if (!newPos.roomPos.equalTo(position.roomPos)) {
                if (Point.manhattanDistance(newPos.roomPos, position.roomPos) > 1) return false;
                if (!moveRoom(newPos.roomPos)) return false;
            }
            
            // make sure there's nothing there
            if (context.appState.entities.anyAt(newPos)) return false;

            var tile = newPos.getTile(context);
            if (!tile.walkable)
                return false; // not Walkable; don't move

            position = newPos;
            return true;
        }

        private bool moveRoom(Point roomPos) {
            // only allow moving to an adjacent room that exists
            if (System.Math.Abs(position.roomPos.x - roomPos.x) == 1 ||
                System.Math.Abs(position.roomPos.y - roomPos.y) == 1) {
                if (context.appState.worldMap[roomPos.x, roomPos.y] != null) {
                    position = new RoomPosition(new Point(roomPos.x, roomPos.y), position.pos);
                }
            }

            return false;
        }
    }
}