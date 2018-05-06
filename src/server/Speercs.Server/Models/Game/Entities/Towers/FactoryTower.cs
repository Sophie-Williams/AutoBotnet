﻿using Speercs.Server.Configuration;
using Speercs.Server.Models.Map;

namespace Speercs.Server.Models.Entities.Towers {
    public class FactoryTower : TowerEntity {
        public FactoryTower(ISContext serverContext, RoomPosition pos) : base(serverContext, pos) { }
    }
}