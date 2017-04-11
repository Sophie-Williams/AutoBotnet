namespace Speercs.Server.Configuration
{
    public class DependencyObject
    {
        public ISContext ServerContext { get; private set; }

        public DependencyObject(ISContext context)
        {
            ServerContext = context;
        }
    }
}