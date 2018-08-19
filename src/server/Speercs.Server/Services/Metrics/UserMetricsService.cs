using LiteDB;
using Speercs.Server.Configuration;
using Speercs.Server.Models.User;

namespace Speercs.Server.Services.Metrics {
    public class UserMetricsService : DependencyObject {
        private readonly LiteCollection<UserMetrics> _metricsCollection;

        public UserMetricsService(ISContext context) : base(context) {
            _metricsCollection = serverContext.database.GetCollection<UserMetrics>(DatabaseKeys.COLLECTION_USERMETRICS);
        }

        public UserMetrics get(string userIdentifier) {
            return _metricsCollection.FindOne(x => x.userId == userIdentifier);
        }

        public UserMetrics create(RegisteredUser user) {
            var userMetrics = new UserMetrics();
            userMetrics.userId = user.identifier;
            _metricsCollection.Insert(userMetrics);
            _metricsCollection.EnsureIndex(x => x.userId);
            return userMetrics;
        }

        public void log(string userIdentifier, MetricsEventType type) {
            var userMetrics = get(userIdentifier);
            var ev = new MetricsEvent {type = type};
            bool log = true;
            if (serverContext.configuration.metricsLevel == MetricsLevel.Minimal) {
                // only log relatively critical information
                log =
                    type == MetricsEventType.CodeDeploy
                    || type == MetricsEventType.Unspecified;
            }

            if (log) userMetrics.events.Add(ev);
            _metricsCollection.Update(userMetrics);
        }

        public bool delete(string userId) {
            return _metricsCollection.Delete(x => x.userId == userId) > 0;
        }
    }
}