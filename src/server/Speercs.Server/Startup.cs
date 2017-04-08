using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Owin;
using Speercs.Server.Configuration;

namespace Speercs.Server
{
    public class Startup
    {
        private const string ConfigFileName = "speercs.json";
        private readonly IConfigurationRoot config;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                              .AddJsonFile(ConfigFileName,
                                optional: true,
                                reloadOnChange: true)
                              .SetBasePath(env.ContentRootPath);

            config = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds services required for using options.
            services.AddOptions();
            // Register IConfiguration
            services.Configure<SConfiguration>(config);
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
            config.Bind(serverConfig);
            // build context
            var context = new SContext(serverConfig);
            
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
