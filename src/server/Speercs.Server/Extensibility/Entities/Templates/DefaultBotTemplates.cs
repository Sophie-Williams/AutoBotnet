using System;
using Speercs.Server.Configuration;
using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Extensibility.Entities.Templates {
    public class ScoutBotTemplate : IBotTemplate {
        public (ResourceId, ulong)[] costs { get; } = new[] {(ResourceId.NRG, 40UL)};
        public string name { get; } = "scout";

        public Bot construct(FactoryTower factory, UserTeam team) {
            return new Bot(factory.position, team, 4);
        }
    }
}