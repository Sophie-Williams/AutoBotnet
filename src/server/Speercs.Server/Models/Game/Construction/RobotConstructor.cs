using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Entities;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;

namespace Speercs.Server.Models.Construction {
    public static class RobotConstructor {
        public static Bot constructBot(ISContext context, FactoryTower factory, string templateName, UserTeam team) {
            var templates = context.extensibilityContainer.resolveAll<IBotTemplate>();
            var template = templates.FirstOrDefault(x => x.name == templateName);
            if (template == null) return null;
            if (!spendResources(team, template.costs)) return null;

            // build the bot
            var bot = template.construct(factory, team);
            context.appState.entities.insertNew(bot);

            return bot;
        }

        public enum BotCoreInstallStatus {
            Success,
            TemplateNotFound,
            InsufficientCapacity,
            InsufficientPower,
            InsufficientResources,
        }

        public static (BotCore, BotCoreInstallStatus) constructCore(ISContext context, Bot bot, string templateName, UserTeam team) {
            var templates = context.extensibilityContainer.resolveAll<IBotCoreTemplate>();
            var template = templates.FirstOrDefault(x => x.name == templateName);
            if (template == null) return (null, BotCoreInstallStatus.TemplateNotFound);
            // ensure bot has space to fit core
            var core = template.construct();
            if (bot.usedCoreSpace + core.size > bot.coreCapacity) {
                return (null, BotCoreInstallStatus.InsufficientCapacity);
            }
            // check reactor power
            if (bot.coreDrain + core.drain > bot.reactorPower) {
                return (null, BotCoreInstallStatus.InsufficientPower);
            }
            if (!spendResources(team, template.costs)) return (null, BotCoreInstallStatus.InsufficientResources);
            // now install the core
            bot.cores.Add(core);
            return (core, BotCoreInstallStatus.Success);
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