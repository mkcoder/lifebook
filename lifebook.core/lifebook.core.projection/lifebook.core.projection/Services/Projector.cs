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
    public abstract class Projector<T> where T: EntityProjection
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
            cache.Set(StreamTrackingInformation, streamInfo);

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
            var mi = cache.Get<Dictionary<string, MethodInfo>>(EventNameToMethodInfo)[subscriptionEvent.Event.EventName];
            var streamInformation = cache.Get<List<StreamTrackingInformation>>(StreamTrackingInformation).FirstOrDefault(m => m.StreamId == subscriptionEvent.StreamInfo);
            Value = await _projectorServices.ProjectionStore.Get<T>(subscriptionEvent.Event.EntityId);
            _projectorServices.StreamTracker.Update(streamInformation.Id, subscriptionEvent.LastStreamEventNumberRead);
            mi.Invoke(this, new object[] { subscriptionEvent.Event });
        }
    }
}
