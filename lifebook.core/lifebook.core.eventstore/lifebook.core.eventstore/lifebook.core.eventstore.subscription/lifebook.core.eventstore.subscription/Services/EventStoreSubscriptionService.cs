using System;
using System.Net;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
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
            connection.ConnectAsync().Wait();
            _eventStoreConfiguration = eventStoreConfiguration;
            _logger = logger;
        }

        public void SubscribeToSingleStream<T>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<T>, Task> action, long? from = null, string subscriptionName = "") where T : ICreateEvent<T>, new()
        {
            var stream = $"$ce-{streamCategory.GetCategoryStream()}";
            var defaultSettings = CatchUpSubscriptionSettings.Default;
            var settings = new CatchUpSubscriptionSettings(defaultSettings.MaxLiveQueueSize, defaultSettings.ReadBatchSize, defaultSettings.VerboseLogging, defaultSettings.ResolveLinkTos, subscriptionName);
            var subscription = connection.SubscribeToStreamFrom(stream, from, CatchUpSubscriptionSettings.Default,
                EventAppeared(action),                
                subscriptionDropped: SubscriptionDropped(streamCategory, action));
        }

        private Action<EventStoreCatchUpSubscription, SubscriptionDropReason, Exception> SubscriptionDropped<T>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<T>, Task> action) where T : ICreateEvent<T>, new()
        {
            return (esc, sdr, ex) =>
            {
                SubscriptionDropped(esc, sdr, ex);
                SubscribeToSingleStream<T>(streamCategory, action);
            };
        }

        private Func<EventStoreCatchUpSubscription, ResolvedEvent, Task> EventAppeared<T>(Func<SubscriptionEvent<T>, Task> action) where T : ICreateEvent<T>, new()
        {
            Func<EventStoreCatchUpSubscription, ResolvedEvent, Task> func = async (es, re) =>
            {
                var subEvent = new SubscriptionEvent<T>();
                subEvent.LastStreamEventNumberRead = re.OriginalEventNumber;
                subEvent.EventNumber = re.Event.EventNumber;
                subEvent.StreamInfo = es.StreamId;
                subEvent.StreamName = es.SubscriptionName;
                subEvent.Event = new T().Create(re.Event.EventType, re.Event.Created, re.Event.Data, re.Event.Metadata);
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