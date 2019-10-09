using System;
using System.Net;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using EventStore.ClientAPI;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.logging.interfaces;
using ILogger = lifebook.core.logging.interfaces.ILogger;

namespace lifebook.core.eventstore.subscription.Services
{
    public class EventStoreSubscriptionService : IEventStoreSubscription
    {
        private IEventStoreConnection connection;
        private readonly EventStoreConfiguration _eventStoreConfiguration;
        private readonly ILogger _logger;

        public EventStoreSubscriptionService(EventStoreConfiguration eventStoreConfiguration, ILogger logger)
        {
            connection = EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Parse(eventStoreConfiguration.IpAddress), eventStoreConfiguration.Port));
            _eventStoreConfiguration = eventStoreConfiguration;
            _logger = logger;
        }

        public void SubscribeToSingleStream<T>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<T>, Task> action, long? from = StreamPosition.Start) where T : ICreateEvent<T>, new()
        {
            var settings = CatchUpSubscriptionSettings.Default;
            connection.SubscribeToStreamFrom(streamCategory.GetCategoryStream(), from, CatchUpSubscriptionSettings.Default, EventAppeared(action), subscriptionDropped: SubscriptionDropped);
        }

        private Func<EventStoreCatchUpSubscription, ResolvedEvent, Task> EventAppeared<T>(Func<SubscriptionEvent<T>, Task> action) where T : ICreateEvent<T>, new()
        {
            Func<EventStoreCatchUpSubscription, ResolvedEvent, Task> func = async (es, re) =>
            {
                var subEvent = new SubscriptionEvent<T>();
                subEvent.EventNumber = re.OriginalEventNumber;
                subEvent.StreamInfo = es.StreamId;
                subEvent.Event = new T().Create(re.OriginalEvent.EventType, re.OriginalEvent.Created, re.OriginalEvent.Data, re.OriginalEvent.Metadata);
                await action(subEvent);
            };

            return func;
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason reason, Exception ex)
        {
            _logger.Error(ex, $"Subscription dropped. Reason: {reason}");
        }
    }
}