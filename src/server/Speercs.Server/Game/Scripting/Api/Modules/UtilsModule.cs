using System;
using System.Collections.Generic;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class UtilsModule : ScriptingApiModule {
        public UtilsModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            double pointDistance(Point p1, Point p2) {
                return Point.dist(p1, p2);
            }

            double distance(RoomPosition p1, RoomPosition p2) {
                return p1.dist(p2);
            }

            RoomPosition[] findPath(RoomPosition start, RoomPosition end) {
                var pathfinder = new Pathfinder(context, start, end);
                var path = pathfinder.findPath(includeGoal: false);
                if (path == null) return new RoomPosition[0];
                return path.ToArray();
            }

            Direction posDirection(RoomPosition source, RoomPosition dest) {
                if (!source.roomPos.equalTo(dest.roomPos)) return Direction.None;
                var dx = dest.pos.x  - source.pos.x;
                var dy = dest.pos.y - source.pos.y;
                if (Math.Abs(dx) >= Math.Abs(dy)) {
                    if (dx >= 0) return Direction.East;
                    else return Direction.West;
                } else {
                    if (dy <= 0) return Direction.North;
                    else return Direction.South;
                }
            }

            defineFunction(nameof(pointDistance), new Func<Point, Point, double>(pointDistance));
            defineFunction(nameof(distance), new Func<RoomPosition, RoomPosition, double>(distance));
            defineFunction(nameof(findPath), new Func<RoomPosition, RoomPosition, RoomPosition[]>(findPath));
            defineFunction(nameof(posDirection), new Func<RoomPosition, RoomPosition, Direction>(posDirection));
        }
    }
}