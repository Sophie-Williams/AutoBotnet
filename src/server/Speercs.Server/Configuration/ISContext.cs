using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Game.Scripting.Engine;
using Speercs.Server.Infrastructure.Concurrency;
using Speercs.Server.Infrastructure.Push;
using Speercs.Server.Services.Application;

namespace Speercs.Server.Configuration {
    public interface ISContext {
        LiteDatabase database { get; }

        SConfiguration configuration { get; }

        UserServiceTable serviceTable { get; }

        SAppState appState { get; }

        CookieJar extensibilityContainer { get; }

        PluginLoader<ISpeercsPlugin> pluginLoader { get; }

        NotificationPipeline notificationPipeline { get; }

        ProgramExecutorManager executors { get; }

        SpeercsLogger log { get; }
    }
}