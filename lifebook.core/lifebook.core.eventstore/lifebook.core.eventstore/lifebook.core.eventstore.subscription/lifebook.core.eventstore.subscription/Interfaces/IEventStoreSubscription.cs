using System;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.eventstore.subscription.Services;

namespace lifebook.core.eventstore.subscription.Interfaces
{
    public interface IEventStoreSubscription
    {
        void SubscribeToSingleStream<T, TOut>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<TOut>, Task> action, long? from = null, string subscriptionName = "", Func<SubscriptionDropped, long> restartFrom = null) where T : ICreateEvent<TOut>, new() where TOut : Event;
    }
}
