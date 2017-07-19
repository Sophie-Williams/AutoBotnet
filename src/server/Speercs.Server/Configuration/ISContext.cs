using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Game.Scripting;
using Speercs.Server.Infrastructure.Concurrency;
using Speercs.Server.Infrastructure.Push;

namespace Speercs.Server.Configuration
{
    public interface ISContext
    {
        LiteDatabase Database { get; }

        SConfiguration Configuration { get; }

        UserServiceTable ServiceTable { get; }

        SAppState AppState { get; }

        CookieJar ExtensibilityContainer { get; }

        PluginLoader<ISpeercsPlugin> PluginLoader { get; }

        NotificationPipeline NotificationPipeline { get; }

        PlayerExecutors Executors { get; }
    }
}
