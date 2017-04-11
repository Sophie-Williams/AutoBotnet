using Speercs.Server.Services.Auth;

namespace Speercs.Server.Models.User
{
    public class ItemCrypto
    {
        public PasswordCryptoConfiguration Conf { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Key { get; set; }
    }
}