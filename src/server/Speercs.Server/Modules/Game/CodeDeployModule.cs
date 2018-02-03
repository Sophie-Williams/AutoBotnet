using Nancy;
using Nancy.ModelBinding;
using Speercs.Server.Configuration;
using Speercs.Server.Models.Game.Program;
using Speercs.Server.Models.Requests;
using Speercs.Server.Modules.User;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game {
    public class CodeDeployModule : UserApiModule {
        public CodeDeployModule(ISContext serverContext) : base("/game/code", serverContext) {
            Get("/get", _ => {
                var program = playerDataService[currentUser.identifier].program;
                return Response.asJsonNet(program);
            });

            Patch("/reload", _ => {
                // reload cached engine for that user
                base.serverContext.executors.reloadExecutor(currentUser.identifier);

                return HttpStatusCode.OK;
            });

            Post("/deploy", _ => {
                var req = this.Bind<CodeDeployRequest>();

                // Validate code size (for security reasons)
                if (req.source.Length > base.serverContext.configuration.codeSizeLimit) {
                    return HttpStatusCode.UnprocessableEntity;
                }

                if (currentUser.analyticsEnabled) {
                    var analyticsObject = base.serverContext.appState.userAnalyticsData[currentUser.identifier];
                    analyticsObject.codeDeploys++;
                    var numLines = (ulong) req.source.Split('\n').Length;
                    analyticsObject.lineCount = numLines;
                    analyticsObject.totalLineCount += numLines;
                }

                playerDataService.deployProgram(currentUser.identifier, new UserProgram(req.source));
                return HttpStatusCode.OK;
            });
        }
    }
}