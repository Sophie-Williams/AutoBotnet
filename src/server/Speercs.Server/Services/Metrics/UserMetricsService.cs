using Speercs.Server.Configuration;
using Speercs.Server.Models.User;

namespace Speercs.Server.Services.Metrics {
    public class UserMetricsService : DependencyObject {
        private readonly string _userIdentifier;

        public UserMetricsService(ISContext context, string userIdentifier) : base(context) {
            _userIdentifier = userIdentifier;
        }

        public UserMetrics get() {
            return serverContext.appState.userMetrics[_userIdentifier];
        }

        public void log(MetricsEventType type) {
            var ev = new MetricsEvent { type = type };
            bool log = true;
            if (serverContext.configuration.metricsLevel == MetricsLevel.Minimal) {
                // only log relatively critical information
                log =
                    type == MetricsEventType.CodeDeploy
                    || type == MetricsEventType.Unspecified;
            }
            if (log) get().events.Add(ev);
        }
    }
}