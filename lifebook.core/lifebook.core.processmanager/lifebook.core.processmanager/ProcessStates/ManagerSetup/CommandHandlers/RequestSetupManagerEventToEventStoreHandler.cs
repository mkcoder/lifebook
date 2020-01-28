using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using lifebook.core.processmanager.Domain;
using MediatR;

namespace lifebook.core.processmanager.ProcessStates
{
    /// <summary>
    /// Subscribe to manager eventstore stream
    /// </summary>
    public class RequestSetupManagerEventToEventStoreHandler : IRequestHandler<RequestSetupManagerEventToEventStore>
    {
        public async Task<Unit> Handle(RequestSetupManagerEventToEventStore request, CancellationToken cancellationToken)
        {
            var domainEventManager = request.ProcessManager.ProcessManagerServices.Container.Resolve<IDomainEventManager>();
            List<EventSpecifier> events = domainEventManager.GetManagerDomainEvents();
            foreach (var domainEvent in events)
            {
                request.ProcessManager.ProcessManagerServices.EventStoreSubscription
                    .SubscribeToSingleStreamWithSubscription<AggregateEventCreator, AggregateEvent>
                    (domainEvent.StreamCategorySpecifier, domainEventManager.DispatchDomainEventToHandler);
            }
            return new Unit();
        }
    }
}
