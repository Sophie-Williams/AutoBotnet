using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Infrastructure.Concurrency;

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
    }
}
