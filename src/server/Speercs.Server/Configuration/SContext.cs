using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Infrastructure.Concurrency;
using Speercs.Server.Infrastructure.Push;

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
        public SAppState AppState { get; set; }

        // Plugin/moddable stuff
        public CookieJar ExtensibilityContainer { get; }

        public PluginLoader<ISpeercsPlugin> PluginLoader { get; }

        public NotificationPipeline NotificationPipeline { get; }

        public static string Version = Microsoft.Extensions.PlatformAbstractions
            .PlatformServices.Default.Application.ApplicationVersion;

        public SContext(SConfiguration config)
        {
            Configuration = config;
            NotificationPipeline = new NotificationPipeline(this);
            ServiceTable = new UserServiceTable(this);
            ExtensibilityContainer = new CookieJar();
            PluginLoader = new PluginLoader<ISpeercsPlugin>();
        }

        public void ConnectDatabase()
        {
            // Create database
            Database = new LiteDatabase(Configuration.DatabaseConfiguration.FileName);
        }
    }
}
