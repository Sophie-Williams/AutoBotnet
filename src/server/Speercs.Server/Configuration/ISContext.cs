using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Infrastructure.Concurrency;
using Speercs.Server.Infrastructure.Push;

namespace Speercs.Server.Configuration {
    public interface ISContext {
        LiteDatabase database { get; }

        SConfiguration configuration { get; }

        UserServiceTable serviceTable { get; }

        SAppState appState { get; }

        CookieJar extensibilityContainer { get; }

        PluginLoader<ISpeercsPlugin> pluginLoader { get; }

        NotificationPipeline notificationPipeline { get; }

        PlayerExecutors executors { get; }
    }
}