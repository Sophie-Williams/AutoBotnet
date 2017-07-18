using Newtonsoft.Json;
using Speercs.Server.Configuration;
using Speercs.Server.Services.Auth;

namespace Speercs.Server.Models
{
    public class PublicMetadata : DependencyObject
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("motd")]
        public string MOTD { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("inviterequired")]
        public bool InviteRequired { get; set; }

        [JsonProperty("usercount")]
        public int UserCount { get; set; }

        public PublicMetadata(ISContext serverContext) : base(serverContext)
        {
            Name = serverContext.Configuration.GlobalName;
            MOTD = serverContext.Configuration.GlobalMessage.Replace("{ver}", SContext.Version);
            Version = SContext.Version;
            InviteRequired = !string.IsNullOrWhiteSpace(serverContext.Configuration.InviteKey);
            UserCount = new UserManagerService(serverContext).RegisteredUserCount;
        }
    }
}
