using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Entities;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;

namespace Speercs.Server.Models.Construction {
    public static class RobotConstructor {
        public static Bot construct(ISContext context, FactoryTower factory, string templateName, UserTeam team) {
            var templates = context.extensibilityContainer.resolveAll<IBotTemplate>();
            var template = templates.FirstOrDefault(x => x.name == templateName);
            if (template == null) return null;
            if (!spendResources(team, template.costs)) return null;

            // build the bot
            var bot = template.construct(factory, team);
            context.appState.entities.insertNew(bot);

            return bot;
        }

        private static bool spendResources(UserTeam team, IEnumerable<(string, ulong)> costs) {
            // ensure that the user can afford
            var costList = costs.ToList();
            foreach (var cost in costList) {
                var (resource, amount) = cost;
                if (team.getResource(resource) < amount) {
                    return false;
                }
            }

            // spend the resources
            foreach (var cost in costList) {
                var (resource, amount) = cost;
                team.resources[resource] -= amount;
            }

            return true;
        }
    }
}