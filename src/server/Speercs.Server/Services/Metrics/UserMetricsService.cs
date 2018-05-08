﻿using Speercs.Server.Configuration;
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

        public MetricsEvent log(MetricsEventType type) {
            var ev = new MetricsEvent { type = type };
            get().events.Add(ev);
            return ev;
        }
    }
}