using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Speercs.Server.Web.Realtime
{
    public interface IRealtimeHandler
    {
        string Path { get; }

        Task<JToken> HandleRequestAsync(long id, JToken data);
    }
}