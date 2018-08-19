using System;
using System.Collections.Generic;
using System.Linq;
using IridiumJS;
using Speercs.Server.Configuration;
using Speercs.Server.Game.Scripting.Api.Refs;
using Speercs.Server.Models.Construction;
using Speercs.Server.Models.Entities;
using Speercs.Server.Models.Entities.Buildings;
using Speercs.Server.Models.Mechanics;
using Speercs.Server.Services.Game;

namespace Speercs.Server.Game.Scripting.Api.Modules {
    public class ArmyModule : ScriptingApiModule {
        public ArmyModule(JSEngine engine, ISContext context, string userId) : base(engine, context, userId) {
            var dataService = new PersistentDataService(context);
            var userData = dataService.get(userId);

            var botConstructor = new RobotConstructor(context, userData.team);

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

            BuildingRef getBuilding(string id) {
                if (id == null) return null;
                var building = userData.team.entities.FirstOrDefault(x => x.id == id) as BuildingEntity;
                if (building == null) return null;
                return new BuildingRef(building);
            }

            BuildingRef getFactory(string id) {
                var factory = userData.team.entities
                    .FirstOrDefault(x => x is FactoryBuilding
                        && ((id == null) || (id == x.id)));
                if (factory == null) return null;
                return new BuildingRef((FactoryBuilding) factory);
            }

            BotEntityRef getBot(string id) {
                if (id == null) return null;
                var bot = userData.team.entities.FirstOrDefault(x => x.id == id) as Bot;
                if (bot == null) return null;
                return new BotEntityRef(bot);
            }

            BotEntityRef constructBot(string templateName, GameEntityRef factoryRef) {
                if (templateName == null) return null;
                var factory = factoryRef?.target as FactoryBuilding;
                if (factory == null) return null;
                // no entities must currently be on top of the factory
                if (context.appState.entities.anyAt(factory.position)) return null;
                var bot = botConstructor.constructBot(factory, templateName);
                if (bot == null) return null;
                userData.team.addEntity(bot);
                return new BotEntityRef(bot);
            }

            bool deconstructBot(BotEntityRef botRef, GameEntityRef factoryRef) {
                if (botRef == null) return false;
                var factory = factoryRef?.target as FactoryBuilding;
                if (factory == null) return false;
                var result = botConstructor.deconstructBot((Bot)botRef.target, factory);
                if (!result) return false;
                userData.team.removeEntity(botRef.target);
                botRef.destroy();
                return true;
            }

            BotCoreRef installCore(string templateName, BotEntityRef botRef) {
                if (templateName == null) return null;
                if (botRef == null) return null;
                var (core, err) = botConstructor.constructCore((Bot)botRef.target, templateName);
                if (core == null) return null;
                return new BotCoreRef(core);
            }

            GameEntityRef[] getUnits() {
                return userData.team.entities.Select(x => new GameEntityRef(x)).ToArray();
            }

            Dictionary<string, long> getResources() {
                return userData.team.resources.Keys.Select(x => new { k = x, v = userData.team.getResource(x) })
                    .ToDictionary(x => x.k, x => x.v);
            }

            defineFunction(nameof(boot), new Func<bool>(boot));
            defineFunction(nameof(getBuilding), new Func<string, BuildingRef>(getBuilding));
            defineFunction(nameof(getFactory), new Func<string, BuildingRef>(getFactory));
            defineFunction(nameof(getBot), new Func<string, GameEntityRef>(getBot));
            defineFunction(nameof(constructBot), new Func<string, GameEntityRef, BotEntityRef>(constructBot));
            defineFunction(nameof(deconstructBot), new Func<BotEntityRef, GameEntityRef, bool>(deconstructBot));
            defineFunction(nameof(installCore), new Func<string, BotEntityRef, BotCoreRef>(installCore));
            defineFunction(nameof(getUnits), new Func<GameEntityRef[]>(getUnits));
            defineFunction(nameof(getResources), new Func<Dictionary<string, long>>(getResources));
        }
    }
}