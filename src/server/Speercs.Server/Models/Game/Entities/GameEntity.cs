
using Speercs.Server.Models.Math;

namespace Speercs.Server.Models.Game.Entities
{
    public abstract class GameEntity
    {
        public string MapIdentifier { get; set; }

        public Point Location { get; set; }

        public Point Size { get; set; }
    }
}