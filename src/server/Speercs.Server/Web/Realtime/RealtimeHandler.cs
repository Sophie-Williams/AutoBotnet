using Newtonsoft.Json.Linq;
using Speercs.Server.Configuration;
using System.Threading.Tasks;

namespace Speercs.Server.Web.Realtime {
    public abstract class RealtimeHandler : DependencyObject, IRealtimeHandler {
        public string path { get; }

        public RealtimeHandler(ISContext context, string path) : base(context) {
            this.path = path;
        }

        public abstract Task<JToken> handleRequestAsync(long id, JToken data, RealtimeContext rtContext);
    }
}