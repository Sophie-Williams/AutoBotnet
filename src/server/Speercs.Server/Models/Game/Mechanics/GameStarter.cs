using System;
using Speercs.Server.Configuration;

namespace Speercs.Server.Models.Mechanics {
    public class GameStarter : DependencyObject {
        public GameStarter(ISContext context) : base(context) { }

        public void bootTeam(UserTeam team) {
            // reset the team
            destroyTeam(team);
            // create a starter factory in a new room
            // TODO: generate a nearby unoccupied room and spawn the factory
            throw new NotImplementedException();
        }

        public void destroyTeam(UserTeam team) {
            foreach (var entity in team.entities) {
                serverContext.appState.entities.remove(entity);
            }

            team.resources.Clear();
        }
    }
}