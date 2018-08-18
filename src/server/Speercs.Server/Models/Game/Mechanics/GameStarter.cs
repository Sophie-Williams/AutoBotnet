using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Mechanics {
    public class GameStarter : DependencyObject {
        public GameStarter(ISContext context) : base(context) { }

        public void bootTeam(UserEmpire team) {
            // reset the team
            destroyTeam(team);
            // give resources
            team.resources["nrg"] = 80;
            team.resources["iron"] = 20;
            // generate a nearby unoccupied room and spawn the factory
            var mapExpander =
                new WorldMapExpander(serverContext, new MapGenerator(serverContext, new MapGenParameters()));
            var homeRoom = mapExpander.createConnectedRoom();
            var factory = new FactoryTower(new RoomPosition(new Point(homeRoom.x, homeRoom.y), homeRoom.spawn), team);
            serverContext.appState.entities.insertNew(factory);
            team.addEntity(factory);
            team.booted = true;
        }

        public void destroyTeam(UserEmpire team) {
            foreach (var entity in team.entities) {
                serverContext.appState.entities.remove(entity);
            }

            team.resources.Clear();
        }
    }
}