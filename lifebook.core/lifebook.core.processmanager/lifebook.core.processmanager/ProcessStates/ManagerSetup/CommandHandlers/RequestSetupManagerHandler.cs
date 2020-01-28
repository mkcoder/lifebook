using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.messagebus.Models;
using lifebook.core.processmanager.Models;
using lifebook.core.processmanager.Services;
using lifebook.core.processmanager.Syntax;
using MediatR;

namespace lifebook.core.processmanager.ProcessStates
{
    public class RequestSetupManagerHandler : IRequestHandler<SetupManager, ManagerSetupCompleted>
    {
        private readonly IMediator _mediator;
        private ProcessManager processmanager;

        public RequestSetupManagerHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ManagerSetupCompleted> Handle(SetupManager request, CancellationToken cancellationToken)
        {
            if(!await _mediator.Send(new AmIManager() { ProcessName = request.ProcessIdentity.ProcessName }))
                return new ManagerSetupCompleted(ProcessIdentity.GetIdentity());

            processmanager = request.ProcessManager;

            foreach (var step in request.ProcessManager.GetConfiguration().GetProcessManagerSteps)
            {
                foreach (var e in step.EventSpecifier)
                {
                    request.ProcessManager.Subscriptions.Add(
                        request.ProcessManager.ProcessManagerServices.EventStoreSubscription.
                        SubscribeToSingleStreamWithSubscription<AggregateEventCreator, AggregateEvent>
                        (e.StreamCategorySpecifier, handle, subscriptionName: $"process_{step.StepDescription.Replace(" ", "")}"));
                }
            }

            await _mediator.Send(new RequestSetupManagerEventToEventStore(request.ProcessManager, request.ProcessIdentity));
            return new ManagerSetupCompleted(request.ProcessIdentity);
        }

        private async Task handle(SubscriptionEvent<AggregateEvent> evt)
        {
            if (processmanager.ProcessManagerServices.EventNameToProcessStepDictionary.ContainsKey(evt.Event.EventName))
            {
                var act = processmanager.ProcessManagerServices.EventNameToProcessStepDictionary[evt.Event.EventName];                
                var bus = processmanager.ProcessManagerServices.Messagebus.TryConnectingDirectlyToQueue(processmanager.ProcessManagerServices.MessageQueueInformation);
                bus.Publish(new ProcessStateMessageDto(evt.Event, act));
            }
        }
    }
}
