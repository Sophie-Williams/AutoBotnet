using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Speercs.Server.Web.Realtime
{
    public interface IRealtimeHandler
    {
        string Path { get; }

        Task<JToken> HandleRequestAsync(long id, JToken data);
    }
}
