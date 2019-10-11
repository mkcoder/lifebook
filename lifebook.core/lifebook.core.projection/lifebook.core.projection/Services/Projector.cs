using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services.StreamTracker;
using Microsoft.Extensions.Caching.Memory;

namespace lifebook.core.projection.Services
{
    public abstract class Projector<T> where T: EntityProjection, new()
    {
        protected T Value { get; private set; }

        private const string EventNameToMethodInfo = "EventNameToMethodInfo";
        private const string StreamTrackingInformation = "StreamTrackingInformation";
        private readonly ProjectorServices _projectorServices;
        private IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        public Projector(ProjectorServices projectorServices)
        {
            _projectorServices = projectorServices;
        }

        public async Task<TResult> Query<TResult>(Func<IEntitySet<T>, Task<TResult>> query)
        {
            return await query(_projectorServices.ProjectionStore.GetEntitySet<T>());
        }

        protected virtual void Start()
        {
            var methods = GetType()
                        .GetMethods()
                        .Where(m => m.GetCustomAttributes(typeof(UponEvent), false).Count() > 0)
                        .Select(m =>
                                    m
                                    .GetCustomAttributes(typeof(UponEvent), false)
                                    .Select(a => ((UponEvent)a).EventName)
                                    .ToDictionary(en => en, mi => m)
                        )
                        .Aggregate(new Dictionary<string, MethodInfo>(), (prev, value) => prev.Union(value).ToDictionary(k => k.Key, v => v.Value));
            cache.Set(EventNameToMethodInfo, methods);
                
            var streamInfo = _projectorServices.StreamTracker.Track(this);

            cache.Set(StreamTrackingInformation, streamInfo.ToDictionary(s => s.StreamId, s => s.Id));

            var streamCategories = GetType()
                            .GetCustomAttributes(typeof(StreamCategory), false)
                            .Select(a => ((StreamCategory)a).ToStreamCategorySpecifier());
            foreach (var item in streamCategories)
            {
                _projectorServices.EventStoreSubscription.SubscribeToSingleStream<AggregateEventCreator, AggregateEvent>(item, EventAction);
            }
        }

        private async Task EventAction(SubscriptionEvent<AggregateEvent> subscriptionEvent)
        {
            _projectorServices.Logger.Information($"{subscriptionEvent}");
            _projectorServices.Logger.Information($"ClassName: {this} RecievedEvent: {subscriptionEvent.StreamName}");
            _projectorServices.Logger.Information($"{subscriptionEvent.Event.EntityId}");
            var eventMethods = cache.Get<Dictionary<string, MethodInfo>>(EventNameToMethodInfo);
            if(eventMethods.ContainsKey(subscriptionEvent.Event.EventName))
            {
                var mi = eventMethods[subscriptionEvent.Event.EventName];
                var streamId = cache.Get<Dictionary<string, Guid>>(StreamTrackingInformation)[subscriptionEvent.StreamInfo.Replace("$ce-", "")];
                var lastSuccessfulHandledEventNumber = _projectorServices.StreamTracker.GetLastEventStoredFromStream(streamId);
                if (subscriptionEvent.LastStreamEventNumberRead > lastSuccessfulHandledEventNumber)
                {
                    _projectorServices.Logger.Information($"Handling Event: {subscriptionEvent.Event.EventName}-{subscriptionEvent.Event.EntityId}");
                    Value = await _projectorServices.ProjectionStore.Get<T>(subscriptionEvent.Event.EntityId) ?? new T() { Key = subscriptionEvent.Event.EntityId };
                    _projectorServices.StreamTracker.Update(streamId, subscriptionEvent.LastStreamEventNumberRead);
                    mi.Invoke(this, new object[] { subscriptionEvent.Event });
                    _projectorServices.ProjectionStore.Store(Value);
                }
            }
        }
    }
}
