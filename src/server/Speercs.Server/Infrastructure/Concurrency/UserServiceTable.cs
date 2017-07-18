using Speercs.Server.Configuration;
using System.Collections.Concurrent;

namespace Speercs.Server.Infrastructure.Concurrency
{
    public class UserServiceTable : DependencyObject
    {
        public UserServiceTable(ISContext serverContext) : base(serverContext)
        {
        }

        private ConcurrentDictionary<string, UserServices> serviceTable = new ConcurrentDictionary<string, UserServices>();

        public UserServices GetOrCreate(string username)
        {
            return serviceTable.GetOrAdd(username, key => {
                return new UserServices(username);
            });
        }

        public UserServices this[string username] => GetOrCreate(username);
    }
}
