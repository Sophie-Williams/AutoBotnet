using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Infrastructure.Concurrency;
using Speercs.Server.Infrastructure.Push;

namespace Speercs.Server.Configuration {
    public class SContext : ISContext {
        // Configuration parameters
        public SConfiguration configuration { get; }

        // Database access
        public LiteDatabase database { get; private set; }

        // Service table
        public UserServiceTable serviceTable { get; }

        // Persistent State
        public SAppState appState { get; set; }

        // Plugin/moddable stuff
        public CookieJar extensibilityContainer { get; }

        public PluginLoader<ISpeercsPlugin> pluginLoader { get; }

        public NotificationPipeline notificationPipeline { get; }

        public PlayerExecutors executors { get; private set; }

        public static string version = Microsoft.Extensions.PlatformAbstractions
            .PlatformServices.Default.Application.ApplicationVersion;

        public SContext(SConfiguration config) {
            configuration = config;
            notificationPipeline = new NotificationPipeline(this);
            serviceTable = new UserServiceTable(this);
            extensibilityContainer = new CookieJar();
            pluginLoader = new PluginLoader<ISpeercsPlugin>();
        }

        public void connectDatabase() {
            // Create database
            database = new LiteDatabase(configuration.databaseConfiguration.fileName);
            // load dependent services
            loadDatabaseDependentServices();
        }

        protected void loadDatabaseDependentServices() {
            executors = new PlayerExecutors(this);
        }
    }
}