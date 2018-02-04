using System.Collections.Concurrent;
using Speercs.Server.Configuration;

namespace Speercs.Server.Infrastructure.Concurrency {
    public class UserServiceTable : DependencyObject {
        public UserServiceTable(ISContext serverContext) : base(serverContext) { }

        private ConcurrentDictionary<string, UserServices> _serviceTable =
            new ConcurrentDictionary<string, UserServices>();

        public UserServices getOrCreate(string username) {
            return _serviceTable.GetOrAdd(username, key => {
                return new UserServices(username);
            });
        }

        public UserServices this[string username] => getOrCreate(username);
    }
}