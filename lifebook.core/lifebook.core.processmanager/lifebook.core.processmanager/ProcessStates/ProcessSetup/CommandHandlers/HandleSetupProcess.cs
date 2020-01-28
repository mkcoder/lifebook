using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using lifebook.core.messagebus.Models;
using lifebook.core.processmanager.Aggregates;
using MediatR;
using Newtonsoft.Json.Linq;

namespace lifebook.core.processmanager.ProcessStates.ProcessSetup.CommandHandlers
{
    public class HandleSetupProcess : IRequestHandler<SetupProcess, ProcessSetupCompleted>
    {
        public HandleSetupProcess()
        {
        }

        public async Task<ProcessSetupCompleted> Handle(SetupProcess request, CancellationToken cancellationToken)
        {
            var configuration = request.ProcessManager.ProcessManagerServices.Configuration.TryGetValueOrDefault("ProcessManagerMode", "ProcessManager");
            if (configuration == "Manager") return new ProcessSetupCompleted();

            var bus = request.ProcessManager.ProcessManagerServices.Messagebus.TryConnectingDirectlyToQueue(request.ProcessManager.ProcessManagerServices.MessageQueueInformation);
            bus.Subscribe<ProcessStateMessageDto>(request.ProcessManager.ProcessManagerServices.MessageQueueInformation, async a =>
            {
                var aggregate = new ProcessManager();
                DetermineProccessIdFromEvent(a.Data.AggregateEvent, out Guid pid);
                var events = await request.ProcessManager.ProcessManagerServices.EventReader
                    .ReadAllEventsFromSingleStreamCategoryAsync<AggregateEventCreator, AggregateEvent>(
                        new StreamCategorySpecifier(request.ProcessManager.ProcessManagerServices.ServiceName, request.ProcessManager.ProcessManagerServices.Instance, "Process", pid)
                    );
                aggregate.Handle(events);
            });
            return new ProcessSetupCompleted();
        }

        private void DetermineProccessIdFromEvent(AggregateEvent a, out Guid pid)
        {
            if (a.ProcessId == null)
                pid = a.EventId;

            pid = a.ProcessId.Value;
        }
    }
}
