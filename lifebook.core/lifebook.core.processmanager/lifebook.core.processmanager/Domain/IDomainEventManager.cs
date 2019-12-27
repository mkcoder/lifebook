using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;

namespace lifebook.core.processmanager.Domain
{
    public interface IDomainEventManager
    {
        List<EventSpecifier> GetManagerDomainEvents();
        Task DispatchDomainEventToHandler(SubscriptionEvent<AggregateEvent> arg);
    }
}
