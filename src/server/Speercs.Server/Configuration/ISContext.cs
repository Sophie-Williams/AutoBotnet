
using LiteDB;
using Speercs.Server.Infrastructure.Concurrency;

namespace Speercs.Server.Configuration
{
    public interface ISContext
    {
        LiteDatabase Database { get; }
        
        SConfiguration Configuration { get; }

        UserServiceTable ServiceTable { get; }
    }
}