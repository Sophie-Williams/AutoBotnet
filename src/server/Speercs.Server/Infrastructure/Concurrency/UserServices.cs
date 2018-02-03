namespace Speercs.Server.Infrastructure.Concurrency {
    public class UserServices {
        public string Username { get; }

        /// <summary>
        /// Read/write concurrency lock system
        /// </summary>
        public UserLock UserLock { get; }

        public UserServices(string username) {
            Username = username;
            UserLock = new UserLock();
        }
    }
}