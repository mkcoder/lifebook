using System;
using EventStore.ClientAPI;

namespace lifebook.core.eventstore.subscription.Services
{
    public class EventStoreSubscriptionHook
    {
        private readonly EventStoreStreamCatchUpSubscription _subscription;

        internal EventStoreSubscriptionHook(EventStoreStreamCatchUpSubscription subscription)
        {
            _subscription = subscription;
        }

        public void Stop() => _subscription.Stop();
        public string GetSubscriptionName => _subscription.SubscriptionName;
        public long GetLastProcessEventNumber => _subscription.LastProcessedEventNumber;
    }
}
