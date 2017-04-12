using LiteDB;
using Speercs.Server.Infrastructure.Concurrency;

namespace Speercs.Server.Configuration
{
    public class SContext : ISContext
    {
        // Configuration parameters
        public SConfiguration Configuration { get; }

        // Database access
        public LiteDatabase Database { get; private set; }

        // Service table
        public UserServiceTable ServiceTable { get; }

        // Persistent State
        public SAppState AppState { get; internal set; }

        public static string Version = Microsoft.Extensions.PlatformAbstractions
            .PlatformServices.Default.Application.ApplicationVersion;

        public SContext(SConfiguration config)
        {
            Configuration = config;
            ServiceTable = new UserServiceTable(this);
        }

        public void ConnectDatabase()
        {
            // Create database
            Database = new LiteDatabase(Configuration.DatabaseConfiguration.FileName);
        }
    }
}