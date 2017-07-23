using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Program;
using Speercs.Server.Models.Requests;
using Speercs.Server.Modules.User;
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

                // Validate code size (for security reasons)
                if (req.Source.Length > ServerContext.Configuration.CodeSizeLimit)
                {
                    return HttpStatusCode.UnprocessableEntity;
                }

                if (CurrentUser.AnalyticsEnabled) {
                    var analyticsObject = ServerContext.AppState.UserAnalyticsData[CurrentUser.Identifier];
                    analyticsObject.CodeDeploys += 1;
                    var numLines = req.Source.Split('\n').Length;
                    analyticsObject.LineCount = numLines;
                    analyticsObject.TotalLineCount += numLines;
                }

                PlayerDataService.DeployProgram(CurrentUser.Identifier, new UserProgram(req.Source));
                return HttpStatusCode.OK;
            });
        }
    }
}
