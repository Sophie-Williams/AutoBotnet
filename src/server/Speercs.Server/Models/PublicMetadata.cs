using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Models
{
    public class PublicMetadata : ProtectedDependencyObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("motd")]
        public string MOTD { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("inviteRequired")]
        public bool InviteRequired { get; set; }

<<<<<<< HEAD
        [JsonProperty("userCount")]
        public int UserCount { get; set; }

        [JsonProperty("tickRate")]
        public int TickRate { get; set; }

        [JsonProperty("mapSize")]
        public int MapSize { get; set; }

=======
>>>>>>> api-impl
        public PublicMetadata(ISContext serverContext) : base(serverContext)
        {
            Name = serverContext.Configuration.GlobalName;
            MOTD = serverContext.Configuration.GlobalMessage.Replace("{ver}", SContext.Version);
            Version = SContext.Version;
            InviteRequired = serverContext.Configuration.InviteRequired;
        }
    }
}
