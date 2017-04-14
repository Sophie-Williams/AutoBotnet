using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.MapGen.Features.Resources
{
    /// <summary>
    /// An implementation of IMapGenFeature for generating resources/ores
    /// </summary>
    public abstract class ResourceFeature : IMapGenFeature
    {
        public virtual void Generate(Room room, IMapGenerator generator)
        {
        }
    }
}