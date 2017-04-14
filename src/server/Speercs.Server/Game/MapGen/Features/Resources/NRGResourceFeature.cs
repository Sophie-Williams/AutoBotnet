using Speercs.Server.Extensibility.MapGen;
using Speercs.Server.Models.Game.Map;

namespace Speercs.Server.Game.MapGen.Features.Resources
{
    public class NRGResourceFeature : ResourceFeature
    {
        public override void Generate(Room room, IMapGenerator generator)
        {
            base.Generate(room, generator);
            
            // TODO: Resource generation post-processing
        }
    }
}