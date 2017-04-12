using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;

namespace Speercs.Server.Web.Realtime
{
    public abstract class RealtimeHandler : DependencyObject, IRealtimeHandler
    {
        public string Path { get; }
        public RealtimeHandler(ISContext context, string path) : base(context)
        {
            Path = path;
        }

        public abstract Task<JToken> HandleRequest(long id, JToken data);
    }
}