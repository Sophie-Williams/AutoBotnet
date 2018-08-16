using System;
using CookieIoC;
using LiteDB;
using Osmium.PluginEngine;
using Speercs.Server.Extensibility;
using Speercs.Server.Game.Scripting.Engine;
using Speercs.Server.Infrastructure.Concurrency;
using Speercs.Server.Infrastructure.Push;
using Speercs.Server.Models.Registry;
using Speercs.Server.Services.Application;
using Speercs.Server.Services.EventPush;

namespace Speercs.Server.Configuration {
    public interface ISContext : IDisposable {
        LiteDatabase database { get; }

        SConfiguration configuration { get; }

        UserServiceTable serviceTable { get; }

        SAppState appState { get; }

        CookieJar extensibilityContainer { get; }

        PluginLoader<ISpeercsPlugin> pluginLoader { get; }

        NotificationPipeline notificationPipeline { get; }

        ProgramExecutorManager executors { get; }

        SpeercsLogger log { get; }

        EventPushService eventPush { get; }
        
        ItemRegistry registry { get; }
    }
}