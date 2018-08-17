using System.Collections.Generic;
using System.Linq;
using Speercs.Server.Configuration;
using Speercs.Server.Extensibility.Economy;

namespace Speercs.Server.Models.Registry {
    public class ResourceRegistry : DependencyObject, IThingRegistry {
        private Dictionary<int, EconomyResource> resourceTypes;

        public ResourceRegistry(ISContext context) : base(context) { }

        public EconomyResource resourceById(int id) {
            return resourceTypes[id];
        }

        public void recache() {
            resourceTypes = serverContext.extensibilityContainer.resolveAll<EconomyResource>()
                .ToDictionary(res => res.id);
        }
    }
}