using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Entities;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Buildings;
using Speercs.Server.Models.Mechanics;

namespace Speercs.Server.Models.Construction {
    public class RobotConstructor : DependencyObject {
        private UserEmpire team;

        public RobotConstructor(ISContext context, UserEmpire team) : base(context) {
            this.team = team;
        }

        public TTemplate resolveTemplate<TTemplate>(string name) where TTemplate : IBotMetaTemplate {
            var templates = serverContext.extensibilityContainer.resolveAll<TTemplate>();
            var template = templates.FirstOrDefault(x => x.name == name);
            return template;
        }

        public Bot constructBot(FactoryBuilding factory, string templateName) {
            var template = resolveTemplate<IBotTemplate>(templateName);
            if (template == null) return null;
            if (!spendResources(template.costs)) return null;
            // build the bot
            var bot = template.construct(factory, team);
            serverContext.appState.entities.insertNew(bot);

            return bot;
        }

        public bool deconstructBot(Bot bot, FactoryBuilding factory) {
            // ensure that the bot is at the factory
            if (bot.position != factory.position) return false;
            // refund resources and destroy the bot
            var template = resolveTemplate<IBotTemplate>(bot.model);
            if (template == null) return false;
            var refundCosts = new List<(string, long)>();
            foreach (var (resource, cost) in template.costs) {
                refundCosts.Add((resource, (long)(-cost * MechanicsConstants.REFUND_FACTOR)));
            }
            if (!spendResources(refundCosts)) return false; // wat?
            // unregister the entity
            serverContext.appState.entities.remove(bot);
            return true;
        }

        public enum BotCoreInstallStatus {
            Success,
            TemplateNotFound,
            InsufficientCapacity,
            InsufficientPower,
            InsufficientResources,
        }

        public (BotCore, BotCoreInstallStatus) constructCore(Bot bot, string templateName) {
            var template = resolveTemplate<IBotCoreTemplate>(templateName);
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
            if (!spendResources(template.costs)) return (null, BotCoreInstallStatus.InsufficientResources);
            // now install the core
            core.bot = bot;
            bot.cores.Add(core);
            return (core, BotCoreInstallStatus.Success);
        }

        private bool spendResources(IEnumerable<(string, long)> costs) {
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