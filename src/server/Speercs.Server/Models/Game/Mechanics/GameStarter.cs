using System;
using Speercs.Server.Configuration;
using Speercs.Server.Game.MapGen;
using Speercs.Server.Models.Entities.Towers;
using Speercs.Server.Models.Map;
using Speercs.Server.Models.Materials;
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Mechanics {
    public class GameStarter : DependencyObject {
        public GameStarter(ISContext context) : base(context) { }

        public void bootTeam(UserTeam team) {
            // reset the team
            destroyTeam(team);
            // give resources
            team.resources[ResourceId.NRG] = 80;
            // create a starter factory in a new room
            // TODO: generate a nearby unoccupied room and spawn the factory
            var mapExpander = new WorldMapExpander(serverContext, new MapGenerator(serverContext, new MapGenParameters()));
            var homeRoom = mapExpander.createConnectedRoom();
            var factory = new FactoryTower(serverContext,
                new RoomPosition(new Point(homeRoom.x, homeRoom.y), homeRoom.spawn), team);
            team.addEntity(factory);
        }

        public void destroyTeam(UserTeam team) {
            foreach (var entity in team.entities) {
                serverContext.appState.entities.remove(entity);
            }

            team.resources.Clear();
        }
    }
}