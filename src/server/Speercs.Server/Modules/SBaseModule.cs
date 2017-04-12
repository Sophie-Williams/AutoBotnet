using Nancy;

namespace Speercs.Server.Modules
{
    /// <summary>
    /// Defines an API module for Speercs
    /// </summary>
    public abstract class SBaseModule : NancyModule
    {
        internal SBaseModule(string path) : base($"/a{path}")
        {
        }
    }
}