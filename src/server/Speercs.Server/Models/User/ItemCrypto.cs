using Speercs.Server.Services.Auth;

namespace Speercs.Server.Models.User {
    public class ItemCrypto {
        public PasswordCryptoConfiguration conf { get; set; }

        public byte[] salt { get; set; }

        public byte[] key { get; set; }
    }
}