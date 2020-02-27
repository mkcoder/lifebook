using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.processmanager.Domain;

namespace lifebook.core.processmanager.Services
{
    public class DomainEventManager : IDomainEventManager
    {
        public DomainEventManager()
        {
        }

        public async Task DispatchDomainEventToHandler(SubscriptionEvent<AggregateEvent> arg)
        {
            // TODO: this should be a case with dispatch to handle
        }

        public List<EventSpecifier> GetManagerDomainEvents()
        {
            return new List<EventSpecifier>() { };
        }
    }
}
