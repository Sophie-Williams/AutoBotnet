using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;

namespace Speercs.Server.Extensibility.Entities.Templates {
    public class ScoutBotTemplate : IBotTemplate {
        public (string, ulong)[] costs { get; } = {("nrg", 40UL)};
        public string name { get; } = "scout";

        public Bot construct(FactoryTower factory, UserTeam team) {
            return new Bot(factory.position, team, 4);
        }
    }
}