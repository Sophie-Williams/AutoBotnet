using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Owin;
using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Services.Application;
using Speercs.Server.Web;

namespace Speercs.Server {
    public class Startup {
        private const string config_file_name = "speercs.json";
        private const string state_storage_database_file_name = "speercs_state.lidb";
        private readonly IConfigurationRoot _fileConfig;

        private SGameBootstrapper gameBootstrapper { get; set; }

        public Startup(IHostingEnvironment env) {
            if (!File.Exists(config_file_name)) {
                try {
                    // Create config file
                    Console.WriteLine($"Configuration file {config_file_name} does not exist, creating default.");
                    var confFileContent = JsonConvert.SerializeObject(new SConfiguration(), Formatting.Indented);
                    File.WriteAllText(config_file_name, confFileContent);
                } catch {
                    Console.WriteLine($"Could not write to {config_file_name}");
                }
            }

            var builder = new ConfigurationBuilder()
                .AddJsonFile(config_file_name,
                    optional: true,
                    reloadOnChange: true)
                .SetBasePath(env.ContentRootPath);

            _fileConfig = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // ReSharper disable once InconsistentNaming
        public void ConfigureServices(IServiceCollection services) {
            // Adds services required for using options.
            services.AddOptions();

            // Register IConfiguration
            services.Configure<SConfiguration>(_fileConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once InconsistentNaming
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            IHostingEnvironment env, ILoggerFactory loggerFactory) {
            // create default configuration
            var serverConfig = new SConfiguration();
            // bind configuration
            _fileConfig.Bind(serverConfig);

            // build context
            var context = SConfigurator.createContext(serverConfig);

            context.log.writeLine("Server context created", SpeercsLogger.LogLevel.Information);

            // load plugins
            // load builtin; TODO: Make some builtin plugins external, load external plugins
            new BuiltinPluginBootstrapper(context).loadAll();

            // load persistent state
            SConfigurator.loadState(context, state_storage_database_file_name);
            context.log.writeLine($"Persistent state loaded from {state_storage_database_file_name}", SpeercsLogger.LogLevel.Information);

            // load database
            context.connectDatabase();
            context.log.writeLine($"Database connected", SpeercsLogger.LogLevel.Information);

            // register application stop handler
            // AssemblyLoadContext.Default.Unloading += (c) => OnUnload(context);
            applicationLifetime.ApplicationStopping.Register(() => onUnload(context));
            context.log.writeLine($"Application interrupt handler registered", SpeercsLogger.LogLevel.Information);

            // add aspnet developer exception page
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            
            // add aspnet logger
            if (env.IsDevelopment() && serverConfig.aspnetVerboseLogging) {
                loggerFactory.AddConsole(LogLevel.Information);
            } else {
                loggerFactory.AddConsole(LogLevel.Warning);
            }

            // map websockets
            app.UseWebSockets();
            app.Map("/ws", ab => WebSocketHandler.map(ab, context));

            // add wwwroot/
            app.UseStaticFiles();

            // set up Nancy OWIN hosting
            app.UseOwin(x => x.UseNancy(options => {
                options.PassThroughWhenStatusCodesAre(
                    HttpStatusCode.NotFound,
                    HttpStatusCode.InternalServerError
                );
                options.Bootstrapper = new SpeercsBootstrapper(context);
            }));

            context.log.writeLine($"Web services mapped successfully", SpeercsLogger.LogLevel.Information);

            // start game services
            gameBootstrapper = new SGameBootstrapper(context);
            gameBootstrapper.onStartup();
        }

        private void onUnload(ISContext sctx) {
            sctx.log.writeLine("Server unloading, force-persisting state data.", SpeercsLogger.LogLevel.Information);
            // persist on unload
            sctx.appState.persist(true);
        }
    }
}