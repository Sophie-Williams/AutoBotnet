using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Owin;
using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Web;
using System;
using System.IO;
using System.Runtime.Loader;

namespace Speercs.Server {
    public class Startup {
        private const string config_file_name = "speercs.json";
        private const string state_storage_database_file_name = "speercs_state.lidb";
        private readonly IConfigurationRoot _fileConfig;

        private string _clientAppPath = "ClientApp/";

        public SGameBootstrapper gameBootstrapper { get; private set; }

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
        public void configureServices(IServiceCollection services) {
            // Adds services required for using options.
            services.AddOptions();

            // Register IConfiguration
            services.Configure<SConfiguration>(_fileConfig);

            // Add AspNetCore MVC
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime,
            IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole();

            // create default configuration
            var serverConfig = new SConfiguration();
            // bind configuration
            _fileConfig.Bind(serverConfig);

            // build context
            var context = SConfigurator.createContext(serverConfig);

            // load plugins
            // load builtin; TODO: Make some builtin plugins external, load external plugins
            new BuiltinPluginBootstrapper(context).loadAll();

            // load persistent state
            SConfigurator.loadState(context, state_storage_database_file_name);

            // load database
            context.connectDatabase();

            // register application stop handler
            // AssemblyLoadContext.Default.Unloading += (c) => OnUnload(context);
            applicationLifetime.ApplicationStopping.Register(() => onUnload(context));

            // map websockets
            app.UseWebSockets();
            app.Map("/ws", (ab) => WebSocketHandler.map(ab, context));

            _clientAppPath = Path.Combine(Directory.GetCurrentDirectory(), _clientAppPath);

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                if (context.configuration.enableDevelopmentWebInterface) {
                    app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                        HotModuleReplacement = true,
                        ProjectPath = _clientAppPath,
                        ConfigFile = $"{_clientAppPath}webpack.config.js"
                    });
                }
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

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

            // set up MVC fallback
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new {controller = "Home", action = "Index"});
            });

            // start game services
            gameBootstrapper = new SGameBootstrapper(context);
            gameBootstrapper.onStartup();
        }

        private void onUnload(ISContext sctx) {
            Console.WriteLine("Server unloading, force-persisting state data.");
            // persist on unload
            sctx.appState.persist(true);
        }
    }
}