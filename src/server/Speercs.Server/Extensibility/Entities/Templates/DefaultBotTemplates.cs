using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Buildings;

namespace Speercs.Server.Extensibility.Entities.Templates {
    public class ScoutBotTemplate : IBotTemplate {
        public (string, long)[] costs { get; } = {("nrg", 40)};
        public string name { get; } = "scout";

        public Bot construct(FactoryBuilding factory, UserEmpire team) {
            return new Bot(factory.position, team) {
                coreCapacity = 4,
                reactorPower = 20,
                memory = new int[32],
                model = name
            };
        }
    }
}