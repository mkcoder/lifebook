using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
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
using Newtonsoft.Json.Linq;
using ILogger = lifebook.core.logging.interfaces.ILogger;

namespace lifebook.core.eventstore.subscription.Services
{
    public class EventStoreSubscriptionService : IEventStoreSubscription
    {
        private IEventStoreConnection connection;
        private readonly EventStoreConfiguration _eventStoreConfiguration;
        private readonly ILogger _logger;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

		public EventStoreSubscriptionService(EventStoreConfiguration eventStoreConfiguration, ILogger logger)
        {
            connection = EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Parse(eventStoreConfiguration.IpAddress), eventStoreConfiguration.Port));
            connection.ConnectAsync().Wait();
            _eventStoreConfiguration = eventStoreConfiguration;
            _logger = logger;
        }

        public void SubscribeToSingleStream<T, TOut>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<TOut>, Task> action, long? from = null, string subscriptionName = null, Func<SubscriptionDropped, long> restartFrom=null) where T : ICreateEvent<TOut>, new() where TOut : Event
        {
            var stream = $"$ce-{streamCategory.GetCategoryStream()}";
            var defaultSettings = CatchUpSubscriptionSettings.Default;
            subscriptionName = subscriptionName ?? Guid.NewGuid().ToString();
            var settings = new CatchUpSubscriptionSettings(defaultSettings.MaxLiveQueueSize, defaultSettings.ReadBatchSize, defaultSettings.VerboseLogging, defaultSettings.ResolveLinkTos, subscriptionName);
            var subscription = connection.SubscribeToStreamFrom(stream, from, settings, 
                EventAppeared<T, TOut>(action),                
                subscriptionDropped: SubscriptionDropped<T, TOut>(streamCategory, action, restartFrom));
            _logger.Information($"Subscription started to stream [SubscriptionName:{subscription.SubscriptionName}]-[StreamId:{subscription.StreamId}]-[LastProcessedEventNumber:{subscription.LastProcessedEventNumber}]");
        }

        public EventStoreSubscriptionHook SubscribeToSingleStreamWithSubscription<T, TOut>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<TOut>, Task> action, long? from = null, string subscriptionName = null, Func<SubscriptionDropped, long> restartFrom = null) where T : ICreateEvent<TOut>, new() where TOut : Event
        {
            var stream = $"$ce-{streamCategory.GetCategoryStream()}";
            var defaultSettings = CatchUpSubscriptionSettings.Default;
            subscriptionName = subscriptionName ?? Guid.NewGuid().ToString();
            var settings = new CatchUpSubscriptionSettings(defaultSettings.MaxLiveQueueSize, defaultSettings.ReadBatchSize, defaultSettings.VerboseLogging, defaultSettings.ResolveLinkTos, subscriptionName);
            var subscription = connection.SubscribeToStreamFrom(stream, from, settings,
                EventAppeared<T, TOut>(action),
                subscriptionDropped: SubscriptionDropped<T, TOut>(streamCategory, action, restartFrom));
            _logger.Information($"Subscription started to stream [SubscriptionName:{subscription.SubscriptionName}]-[StreamId:{subscription.StreamId}]-[LastProcessedEventNumber:{subscription.LastProcessedEventNumber}]");
            return new EventStoreSubscriptionHook(subscription);
        }

        private Action<EventStoreCatchUpSubscription, SubscriptionDropReason, Exception> SubscriptionDropped<T, TOut>(StreamCategorySpecifier streamCategory, Func<SubscriptionEvent<TOut>, Task> action, Func<SubscriptionDropped, long> restartFrom) where T : ICreateEvent<TOut>, new() where TOut : Event
        {
            return (esc, sdr, ex) =>
            {
                SubscriptionDropped(esc, sdr, ex);
                var subDropped = new SubscriptionDropped()
                {
                    SubscriptionName = esc.SubscriptionName,
                    StreamId = esc.StreamId,
                    Reason = sdr.ToString(),
                    Message = ex.Message
                };
                SubscribeToSingleStream<T, TOut>(streamCategory, action, from: restartFrom(subDropped), subscriptionName: esc.SubscriptionName, restartFrom: restartFrom);
            };
        }

        private Func<EventStoreCatchUpSubscription, ResolvedEvent, Task> EventAppeared<T, TOut>(Func<SubscriptionEvent<TOut>, Task> action) where T : ICreateEvent<TOut>, new() where TOut : Event
        {
            Func<EventStoreCatchUpSubscription, ResolvedEvent, Task> func = async (es, re) =>
            {
                await semaphoreSlim.WaitAsync();
				try
				{
                    var subEvent = new SubscriptionEvent<TOut>();
                    subEvent.LastStreamEventNumberRead = re.OriginalEventNumber;
                    subEvent.EventNumber = re.Event.EventNumber;
                    subEvent.StreamInfo = es.StreamId;
                    subEvent.StreamName = es.SubscriptionName;
                    subEvent.Event = new T().Create(re.Event.EventType, re.Event.Created, re.Event.Data, re.Event.Metadata);
                    _logger.Information($"Event Recieved: {JObject.FromObject(subEvent).ToString()}");
                    await action(subEvent);
                }
				finally
				{
                    semaphoreSlim.Release();
				}
            };

            return func;
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason reason, Exception ex)
        {
            _logger.Error(ex, $"Subscription dropped. Reason: {reason}");
        }
    }
}