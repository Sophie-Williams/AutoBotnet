using Speercs.Server.Configuration;
using Speercs.Server.Utilities;

namespace Speercs.Server.Modules.Game
{
    public class CodeDeployModule : UserApiModule
    {
        public CodeDeployModule(ISContext serverContext) : base("/game/code", serverContext)
        {
            Post("/get", _ => 
            {
                var program = (serverContext.AppState.PlayerData[CurrentUser.Username]).Program;
                return Response.AsJsonNet(program);
            });
        }
    }
}
