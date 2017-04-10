
using LiteDB;

namespace Speercs.Server.Configuration
{
    public interface ISContext
    {
        LiteDatabase Database { get; }
        
        SConfiguration Configuration { get; }
    }
}