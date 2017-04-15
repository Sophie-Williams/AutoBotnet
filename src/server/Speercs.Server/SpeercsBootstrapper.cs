using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

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

            // Enable stateless authentication
            StatelessAuthentication.Enable(pipelines, new StatelessAuthenticationConfiguration(ctx =>
            {
                // Take API from query string
                var apiKey = (string)ctx.Request.Query.apikey.Value;
                // call authenticator
                var auther = new ApiAuthenticator(ServerContext);
                return auther.ResolveIdentity(apiKey);
            }));

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
