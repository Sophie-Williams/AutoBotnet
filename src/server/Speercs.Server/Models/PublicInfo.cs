using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Models
{
    public class PublicInfo : ProtectedDependencyObject
    {
        [JsonProperty("userCount")]
        public int UserCount { get; set; }

        [JsonProperty("tickrate")]
        public int TickRate { get; set; }

        [JsonProperty("mapsize")]
        public int MapSize { get; set; }

        public PublicInfo(ISContext serverContext) : base(serverContext)
        {
            UserCount = new UserManagerService(serverContext).RegisteredUserCount;
            TickRate = serverContext.Configuration.TickRate;
            MapSize = serverContext.AppState.WorldMap.RoomCount;
        }
    }
}
