using Speercs.Server.Configuration;

namespace Speercs.Server.Models.Registry {
    public interface IThingRegistry {
        void recache();
    }

    public class ItemRegistry : DependencyObject, IThingRegistry {
        public TileRegistry tiles;

        public ItemRegistry(ISContext context) : base(context) {
            tiles = new TileRegistry(context);
        }

        public void recache() {
            tiles.recache();
        }
    }
}