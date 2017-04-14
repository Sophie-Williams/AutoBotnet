using Nancy;
using Speercs.Server.Configuration;

namespace Speercs.Server.Modules.Admin
{
    public class AdminTestModule : AdminApiModule
    {
        public AdminTestModule(ISContext serverContext) : base("/admin/test", serverContext)
        {
            Get("/", _ => HttpStatusCode.OK);
        }
    }
}
