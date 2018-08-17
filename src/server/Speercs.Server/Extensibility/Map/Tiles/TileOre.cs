using System;
using SixLabors.ImageSharp.PixelFormats;
using Speercs.Server.Extensibility.Economy;
using Speercs.Server.Extensibility.Economy.Resources;

namespace Speercs.Server.Extensibility.Map.Tiles {
    public class TileOre : Tile {
        public override bool walkable => false;
        public override bool mineable => true;
        public override char tileChar => '*';
        public override Rgba32 color => Rgba32.Gold;
        public override string name => "ore";

        public const int PROP_ORE_TYPE = 0x07;
        public const int PROP_ORE_AMOUNT = 0x08;

        public long oreType {
            get { return props.get(PROP_ORE_TYPE); }
            set { props.set(PROP_ORE_TYPE, value); }
        }

        public long oreAmount {
            get { return props.get(PROP_ORE_AMOUNT); }
            set { props.set(PROP_ORE_AMOUNT, value); }
        }

        public override bool drill(DrillContext context) {
            if (oreAmount <= 0) return false;
            var resource = context.serverContext.registry.resources.resourceById((int) oreType);
            var drilled = Math.Min(oreAmount, resource.chunk);
            oreAmount -= drilled;
            context.outputs[resource.name] += drilled;
            return true;
        }

        public static TileOre create(OreType ore, long amount) {
            var tile = new TileOre();
            tile.durability = ore.durability;
            tile.oreType = ore.resource.id;
            tile.oreAmount = amount;
            return tile;
        }
    }

    public abstract class OreType {
        public abstract int durability { get; }
        public abstract EconomyResource resource { get; }
    }

    public class NRGOre : OreType {
        public override int durability => 1;
        public override EconomyResource resource => new NRGResource();
    }
}