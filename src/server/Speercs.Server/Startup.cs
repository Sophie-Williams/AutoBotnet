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

namespace Speercs.Server
{
    public class Startup
    {
        private const string ConfigFileName = "speercs.json";
        private const string StateStorageDatabaseFileName = "speercs_state.lidb";
        private readonly IConfigurationRoot fileConfig;

        private string ClientAppPath = "ClientApp/";

        public SGameBootstrapper GameBootstrapper { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            if (!File.Exists(ConfigFileName))
            {
                try
                {
                    // Create config file
                    var confFileContent = JsonConvert.SerializeObject(new SConfiguration(), Formatting.Indented);
                    File.WriteAllText(ConfigFileName, confFileContent);
                }
                catch
                {
                    Console.WriteLine($"Could not write to {ConfigFileName}");
                }
            }
            var builder = new ConfigurationBuilder()
                              .AddJsonFile(ConfigFileName,
                                optional: true,
                                reloadOnChange: true)
                              .SetBasePath(env.ContentRootPath);

            fileConfig = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds services required for using options.
            services.AddOptions();

            // Register IConfiguration
            services.Configure<SConfiguration>(fileConfig);

            // Add AspNetCore MVC
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            // create default configuration
            var serverConfig = new SConfiguration();
            // bind configuration
            fileConfig.Bind(serverConfig);

            // build context
            var context = SConfigurator.CreateContext(serverConfig);

            // load plugins
            // load builtin; TODO: Make some builtin plugins external, load external plugins
            new BuiltinPluginBootstrapper(context).LoadAll();

            // load persistent state
            SConfigurator.LoadState(context, StateStorageDatabaseFileName);

            // load database
            context.ConnectDatabase();

            // register application stop handler
            // AssemblyLoadContext.Default.Unloading += (c) => OnUnload(context);
            applicationLifetime.ApplicationStopping.Register(() => OnUnload(context));

            // map websockets
            app.UseWebSockets();
            app.Map("/ws", (ab) => WebSocketHandler.Map(ab, context));

            ClientAppPath = Path.Combine(Directory.GetCurrentDirectory(), ClientAppPath);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                if (context.Configuration.EnableDevelopmentWebInterface) {
                    app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                    {
                        HotModuleReplacement = true,
                        ProjectPath = ClientAppPath,
                        ConfigFile = $"{ClientAppPath}webpack.config.js"
                    });
                }
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // add wwwroot/
            app.UseStaticFiles();

            // set up Nancy OWIN hosting
            app.UseOwin(x => x.UseNancy(options =>
            {
                options.PassThroughWhenStatusCodesAre(
                    HttpStatusCode.NotFound,
                    HttpStatusCode.InternalServerError
                );
                options.Bootstrapper = new SpeercsBootstrapper(context);
            }));

            // set up MVC fallback
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            // start game services
            GameBootstrapper = new SGameBootstrapper(context);
            GameBootstrapper.OnStartup();
        }

        private void OnUnload(ISContext sctx)
        {
            Console.WriteLine("Server unloading, force-persisting state data.");
            // persist on unload
            sctx.AppState.Persist(true);
        }
    }
}
