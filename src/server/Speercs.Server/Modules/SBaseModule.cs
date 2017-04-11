using Nancy;

namespace Speercs.Server.Modules
{
    public abstract class SBaseModule : NancyModule
    {
        internal SBaseModule(string path) : base($"/a{path}")
        {
        }
    }
}