using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.orchestrator.Interfaces;
using lifebook.core.orchestrator.Models;

namespace lifebook.core.orchestrator.Services
{
    public abstract class EventOrchestration : AbstrateOrchestrate
    {
        private readonly IEventStoreSubscription _eventStoreSubscriptionService;
        private EventSpecifier _eventSpecifier;

        public EventOrchestration(IEventStoreSubscription eventStoreSubscriptionService)
        {
            _eventStoreSubscriptionService = eventStoreSubscriptionService;
        }

        public abstract EventSpecifier GetEventSpecifier();
        public abstract Task Orchestrate(AggregateEvent aggregateEvent);

        internal override async Task Run()
        {
            _eventSpecifier = GetEventSpecifier();
            _eventStoreSubscriptionService.SubscribeToSingleStream<AggregateEventCreator, AggregateEvent>(_eventSpecifier.StreamCategorySpecifier, uponEventCB);
        }

        private async Task uponEventCB(SubscriptionEvent<AggregateEvent> evt)
        {
            if(evt.Event.EventName == _eventSpecifier.EventName)
            {
               await Orchestrate(evt.Event);
            }
        }
    }
}
