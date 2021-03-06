using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Models.Meta {
    public class PublicInfo : ProtectedDependencyObject {
        [JsonProperty("userCount")]
        public int userCount { get; set; }

        [JsonProperty("tickrate")]
        public int tickrate { get; set; }

        [JsonProperty("mapSize")]
        public int mapSize { get; set; }

        [JsonProperty("inviteRequired")]
        public bool inviteRequired { get; set; }

        public PublicInfo(ISContext serverContext) : base(serverContext) {
            userCount = new UserManagerService(serverContext).registeredUserCount;
            tickrate = serverContext.configuration.tickrate;
            mapSize = serverContext.appState.worldMap.roomCount;
            inviteRequired = serverContext.configuration.inviteRequired;
        }
    }
}