using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Entities.Buildings;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Mechanics {
    public class GameManager : DependencyObject {
        public GameManager(ISContext context) : base(context) { }

        public bool bootTeam(UserEmpire team) {
            if (team.bootTime > 0) {
                // enforce the boot limit
                if (serverContext.appState.tickCount - team.bootTime < serverContext.configuration.armyBootCooldown)
                    return false;
            }

            // reset the team
            destroyTeam(team);
            // give resources
            team.resources["nrg"] = 80;
            team.resources["iron"] = 20;
            // generate a nearby unoccupied room and spawn the factory
            var mapExpander =
                new WorldMapExpander(serverContext, new MapGenerator(serverContext, new MapGenParameters()));
            var homeRoom = mapExpander.createConnectedRoom();
            var factory =
                new FactoryBuilding(new RoomPosition(new Point(homeRoom.x, homeRoom.y), homeRoom.spawn), team);
            serverContext.appState.entities.insertNew(factory);
            team.addEntity(factory);
            team.bootTime = serverContext.appState.tickCount;
            team.booted = true;
            return true;
        }

        public void destroyTeam(UserEmpire team) {
            var entityList = team.entities.ToList();
            foreach (var entity in entityList) {
                serverContext.appState.entities.remove(entity);
            }

            team.resources.Clear();
            team.booted = false;
        }
    }
}