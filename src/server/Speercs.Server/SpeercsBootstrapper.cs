using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;
using System.Security.Claims;
using System.Security.Principal;

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

                // get user identity
                var userManager = new UserManagerService(ServerContext);
                var user = userManager.FindUserByApiKeyAsync(apiKey).Result;
                return new ClaimsPrincipal(new GenericIdentity(user.Identifier));
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