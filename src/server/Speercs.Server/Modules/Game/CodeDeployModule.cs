using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Program;
using Speercs.Server.Models.Requests;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game
{
    public class CodeDeployModule : UserApiModule
    {
        public CodeDeployModule(ISContext serverContext) : base("/game/code", serverContext)
        {
            Get("/get", _ => 
            {
                var program = (serverContext.AppState.PlayerData[CurrentUser.Username]).Program;
                return Response.AsJsonNet(program);
            });

            Post("/deploy", _ => 
            {
                var req = this.Bind<CodeDeployRequest>();
                serverContext.AppState.PlayerData[CurrentUser.Username].Program = new UserProgram(req.Source);
                return HttpStatusCode.OK;
            });
        }
    }
}
