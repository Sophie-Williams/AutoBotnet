using System;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class UtilsModule : ScriptingApiModule {
        public UtilsModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            double pointDistance(Point p1, Point p2) {
                return Point.distance(p1, p2);
            }

            defineFunction("pointDistance", new Func<Point, Point, double>(pointDistance));
        }
    }
}