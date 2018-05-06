using System;
using System.IO;
using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Game.Scripting.Engine;
using Speercs.Server.Infrastructure.Concurrency;
using Speercs.Server.Infrastructure.Push;
using Speercs.Server.Services.Application;

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

        public ProgramExecutorManager executors { get; private set; }

        public SpeercsLogger log { get; }

        public const string version = "0.0.3-dev";

        public SContext(SConfiguration config) {
            configuration = config;
            notificationPipeline = new NotificationPipeline(this);
            serviceTable = new UserServiceTable(this);
            extensibilityContainer = new CookieJar();
            pluginLoader = new PluginLoader<ISpeercsPlugin>();
            log = new SpeercsLogger(config.logLevel);
        }

        public void connectDatabase() {
            // Create database
            if (configuration.databaseConfiguration.fileName == null) {
                database = new LiteDatabase(configuration.databaseConfiguration.fileName);
            } else {
                log.writeLine("Database target file not provided, using transient in-memory storage backend", SpeercsLogger.LogLevel.Warning);
                database = new LiteDatabase(new MemoryStream());
            }
            // load dependent services
            loadDatabaseDependentServices();
        }

        private void loadDatabaseDependentServices() {
            executors = new ProgramExecutorManager(this);
        }

        public void Dispose() {
            database?.Dispose();
        }
    }
}