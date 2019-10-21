using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
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
    public interface IProjector { }

    public abstract class Projector<T> : IProjector where T: EntityProjection, new()
    {
        protected T Value { get; private set; }

        private Dictionary<string, MethodInfo> methods;
        private readonly ProjectorServices _projectorServices;
        private Dictionary<string, Guid> streamInfo;
        private readonly static object _lock = new object();

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
            methods = GetType()
                    .GetMethods()
                    .Where(m => m.GetCustomAttributes(typeof(UponEvent), false).Count() > 0)
                    .Select(m =>
                                m
                                .GetCustomAttributes(typeof(UponEvent), false)
                                .Select(a => ((UponEvent)a).EventName)
                                .ToDictionary(en => en, mi => m)
                    )
                    .Aggregate(new Dictionary<string, MethodInfo>(), (prev, value) => prev.Union(value).ToDictionary(k => k.Key, v => v.Value));

            var streamData = _projectorServices.StreamTracker.Track(this);
            var streamInfo2 = streamData.ToDictionary(s => s.StreamId, s => s.LastEventRead);
            streamInfo = streamData.ToDictionary(s => s.StreamId, s => s.Id);

            var streamCategories = GetType()
                            .GetCustomAttributes(typeof(StreamCategory), false)
                            .Select(a => ((StreamCategory)a).ToStreamCategorySpecifier());
            foreach (var item in streamCategories)
            {
                var evtNumber = streamInfo2[item.GetCategoryStream()];
                _projectorServices.EventStoreSubscription.SubscribeToSingleStream<AggregateEventCreator, AggregateEvent>(item, EventAction, evtNumber);
            }
        }

        private async Task EventAction(SubscriptionEvent<AggregateEvent> subscriptionEvent)
        {
            try
            {
                _projectorServices.Logger.Information($"{subscriptionEvent}");
                _projectorServices.Logger.Information($"Projector: {GetType().FullName} StreamName: {subscriptionEvent.StreamName} EventName: {subscriptionEvent.Event.EventName}");
                _projectorServices.Logger.Information($"{subscriptionEvent.Event.EntityId}");
                if (methods.ContainsKey(subscriptionEvent.Event.EventName))
                {
                    var services = _projectorServices.Container.Resolve<ProjectorServices>();
                    var mi = methods[subscriptionEvent.Event.EventName];
                    var streamId = streamInfo[subscriptionEvent.StreamInfo.Replace("$ce-", "")];
                    var lastSuccessfulHandledEventNumber = services.StreamTracker.GetLastEventStoredFromStream(streamId);
                    if (subscriptionEvent.LastStreamEventNumberRead >= lastSuccessfulHandledEventNumber)
                    {
                        _projectorServices.Logger.Information($"Handling Event: {subscriptionEvent.Event.EventName}-{subscriptionEvent.Event.EntityId}");
                        var entry = await services.ProjectionStore.GetAsync<T>(subscriptionEvent.Event.EntityId);
                        if (entry == default(T))
                            Value = new T { Key = subscriptionEvent.Event.EntityId };
                        else
                            Value =  entry;

                        mi.Invoke(this, new object[] { subscriptionEvent.Event });
                        services.ProjectionStore.Store(Value);
                        services.StreamTracker.Update(streamId, subscriptionEvent.LastStreamEventNumberRead);
                    }
                }
            }
            catch (Exception ex)
            {
                var exm = ex.Message;
            }
        }
    }
}
