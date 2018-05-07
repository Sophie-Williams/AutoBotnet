using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Requests.Game;
using Speercs.Server.Services.Application;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Admin {
    public class AdminServerModule : AdminApiModule {
        public AdminServerModule(ISContext serverContext) : base("/server", serverContext) {
            Post("/persist", _ => {
                serverContext.appState.persist(true);
                return HttpStatusCode.NoContent;
            });
        }
    }
}