using System;
using System.Diagnostics;
using Speercs.Server.Models.Entities.Buildings;

namespace Speercs.Server.Game.Scripting.Api.Refs {
    public class BuildingRef : GameEntityRef {
        private BuildingEntity _building;

        public BuildingRef(BuildingEntity building) : base(building) {
            _building = building;
        }

        public object call(string name, params object[] args) {
            if (_building.actions.ContainsKey(name)) {
                try {
                    return _building.actions[name].DynamicInvoke(args);
                } catch (Exception callException) {
                    Trace.WriteLine(callException);
                }
            }

            return null;
        }
    }
}