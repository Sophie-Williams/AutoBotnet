using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Entities;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Materials;

namespace Speercs.Server.Models.Construction {
    public static class RobotConstructor {
        public static Bot construct(ISContext context, FactoryTower factory, string templateName, UserTeam team) {
            var templates = context.extensibilityContainer.ResolveAll<IBotTemplate>();
            var template = templates.FirstOrDefault(x => x.name == templateName);
            if (template == null) return null;
            if (!spendResources(team, template.costs)) return null;

            // build the bot
            var bot = template.construct(context, factory, team);

            return bot;
        }

        private static bool spendResources(UserTeam team, IEnumerable<(ResourceId, ulong)> costs) {
            // ensure that the user can afford
            var costList = costs.ToList();
            foreach (var cost in costList) {
                (ResourceId resource, ulong amount) = cost;
                if (team.resources[resource] < amount) {
                    return false;
                }
            }

            // spend the resources
            foreach (var cost in costList) {
                (ResourceId resource, ulong amount) = cost;
                team.resources[resource] -= amount;
            }

            return true;
        }
    }
}