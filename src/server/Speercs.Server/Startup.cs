using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Owin;
using Speercs.Server.Configuration;
using Speercs.Server.Game;
using Speercs.Server.Web;

namespace Speercs.Server
{
    public class Startup
    {
        private const string ConfigFileName = "speercs.json";
        private const string ExampleConfigName = "speercs.example.json";
        private const string StateStorageDatabaseFileName = "speercs_state.lidb";
        private readonly IConfigurationRoot fileConfig;

        public Startup(IHostingEnvironment env)
        {
            if (!File.Exists(ConfigFileName) && File.Exists(ExampleConfigName))
            {
                Console.WriteLine($"{ConfigFileName} does not exist. Creating from template");
                File.Copy(ExampleConfigName, ConfigFileName);
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            // map websockets
            app.UseWebSockets();
            app.Map("/ws", (ab) => WebSocketHandler.Map(ab, context));

            // set up Nancy OWIN hosting
            app.UseOwin(x => x.UseNancy(options =>
            {
                options.PassThroughWhenStatusCodesAre(
                    HttpStatusCode.NotFound,
                    HttpStatusCode.InternalServerError
                );
                options.Bootstrapper = new SpeercsBootstrapper(context);
            }));
        }
    }
}
