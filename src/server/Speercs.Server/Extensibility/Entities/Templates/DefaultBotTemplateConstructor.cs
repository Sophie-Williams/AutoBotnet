using Speercs.Server.Models;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Buildings;

namespace Speercs.Server.Extensibility.Entities.Templates {
    public static class DefaultBotTemplateConstructor {
        public static Bot construct(FactoryBuilding factory, UserEmpire team, IBotTemplate template) {
            var bot = new Bot(factory.position, team) {
                model = template.name,
                coreCapacity = template.coreCapacity,
                reactorPower = template.reactorPower,
                memory = new int[template.memorySize],
                moveCost = template.movecost
            };
            bot.resetHealth(template.health);
            return bot;
        }
    }
}