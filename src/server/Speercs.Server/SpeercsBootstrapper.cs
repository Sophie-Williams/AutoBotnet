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
                // Take API key from query string
                var token = (string)ctx.Request.Query.token.Value;
                if (token == null) // key wasn't in query, check alternate sources
                {
                    var authHeader = ctx.Request.Headers.Authorization;
                    if (!string.IsNullOrWhiteSpace(authHeader))
                    {
                        token = authHeader;
                    }
                }
                // call authenticator
                var auther = new ApiAuthenticator(ServerContext);
                return auther.ResolveIdentity(token);
            }));

            // Enable CORS
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                foreach (var origin in ServerContext.Configuration.CorsOrigins)
                {
                    ctx.Response.WithHeader("Access-Control-Allow-Origin", origin);
                }
                ctx.Response
                    .WithHeader("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type, Authorization");
            });

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
