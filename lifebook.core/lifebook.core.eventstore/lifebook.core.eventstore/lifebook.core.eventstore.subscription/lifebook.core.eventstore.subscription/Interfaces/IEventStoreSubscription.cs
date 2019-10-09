using System;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;

namespace lifebook.core.eventstore.subscription.Interfaces
{
    public interface IEventStoreSubscription
    {
        void SubscribeToSingleStream<T>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<T>, Task> action, long? from = null) where T : ICreateEvent<T>, new();
    }
}
