using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;

namespace Speercs.Server.Web.Realtime
{
    public abstract class RealtimeHandler : DependencyObject
    {
        public string Path { get; }
        public RealtimeHandler(ISContext context, string path) : base(context)
        {
            Path = path;
        }
        
        public abstract JObject HandleRequest(long id, JObject data);
    }
}