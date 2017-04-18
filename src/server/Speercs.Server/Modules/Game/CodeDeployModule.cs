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
                var program = PlayerDataService[CurrentUser.Identifier].Program;
                return Response.AsJsonNet(program);
            });

            Patch("/reload", _ =>
            {
                // reload cached engine for that user
                ServerContext.Executors.ReloadExecutor(CurrentUser.Identifier);

                return HttpStatusCode.OK;
            });

            Post("/deploy", _ =>
            {
                var req = this.Bind<CodeDeployRequest>();

                PlayerDataService.DeployProgram(CurrentUser.Identifier, new UserProgram(req.Source));
                return HttpStatusCode.OK;
            });
        }
    }
}
