using System;
using System.Collections.Generic;
using System.Linq;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api.Refs;
using Speercs.Server.Models.Construction;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Mechanics;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ArmyModule : ScriptingApiModule {
        public ArmyModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            var dataService = new PersistentDataService(context);
            var userData = dataService.get(userId);

            bool boot() {
                // "boot up", or create the player's force
                if (userData.team.booted) {
                    // can't "boot" an already-booted team
                    return false;
                }

                var starter = new GameStarter(context);
                starter.bootTeam(userData.team);
                return true;
            }

            GameEntityRef getFactory(int seq) {
                var factories = userData.team.entities.Where(x => x is FactoryTower).ToList();
                if (!factories.Any()) return null;
                return new GameEntityRef(engine, (FactoryTower) factories[seq]);
            }

            BotEntityRef getBot(string id) {
                var bot = userData.team.entities.FirstOrDefault(x => x.id == id) as Bot;
                if (bot == null) return null;
                return new BotEntityRef(engine, bot);
            }

            bool constructBot(string templateName, GameEntityRef factoryRef) {
                var factory = factoryRef.target as FactoryTower;
                if (factory == null) return false;
                var bot = RobotConstructor.constructBot(context, factory, templateName, userData.team);
                if (bot == null) return false;
                userData.team.addEntity(bot);
                return true;
            }

            BotCoreRef installCore(string templateName, BotEntityRef botRef) {
                var core = RobotConstructor.constructCore(context, (Bot) botRef.target, templateName, userData.team);
                if (core == null) return null;
                return new BotCoreRef(engine, core);
            }

            GameEntityRef[] getUnits() {
                return userData.team.entities.Select(x => new GameEntityRef(engine, x)).ToArray();
            }

            ulong getResource(string resource) {
                return userData.team.getResource(resource);
            }

            defineFunction(nameof(boot), new Func<bool>(boot));
            defineFunction(nameof(getFactory), new Func<int, GameEntityRef>(getFactory));
            defineFunction(nameof(getBot), new Func<string, GameEntityRef>(getBot));
            defineFunction(nameof(constructBot), new Func<string, GameEntityRef, bool>(constructBot));
            defineFunction(nameof(installCore), new Func<string, BotEntityRef, BotCoreRef>(installCore));
            defineFunction(nameof(getUnits), new Func<GameEntityRef[]>(getUnits));
            defineFunction(nameof(getResource), new Func<string, ulong>(getResource));
        }
    }
}