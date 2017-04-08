using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Speercs.Server.Configuration;

namespace Speercs.Server
{
    public class SpeercsBootstrapper : DefaultNancyBootstrapper
    {
        public SContext ServerContext { get; }
        public SpeercsBootstrapper(SContext context)
        {
            ServerContext = context;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            // TODO: Load database, etc.
            ServerContext.ConnectDatabase();

            // TODO: Set configuration
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            // Register IoC components
            container.Register<ISContext>(ServerContext);
        }
    }
}