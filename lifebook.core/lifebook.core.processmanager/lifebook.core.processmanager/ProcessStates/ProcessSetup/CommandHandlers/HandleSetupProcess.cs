using System;
using System.Collections.Generic;
using System.Linq;
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
                DetermineProccessIdFromEvent(a.Data.AggregateEvent, out Guid pid);
                var aggregate = new ProcessManager(pid);
                var category =
                        new StreamCategorySpecifier(request.ProcessManager.
                        ProcessManagerServices.ServiceName, request.ProcessManager.ProcessManagerServices.Instance, "Process", pid);
                var events = await request.ProcessManager.ProcessManagerServices.EventReader
                    .ReadAllEventsFromSingleStreamCategoryAsync<AggregateEventCreator, AggregateEvent>(category
                    );
                aggregate.Handle(events);
                request.ProcessManager.ViewBag = aggregate.Data;

                if (aggregate.AmIFirstStep())
                {
                    aggregate.InitalizeProcess(a, request);
                }
                
                if (request.ProcessManager.EventNameToProcessStepDictionary.ContainsKey(a.Data.AggregateEvent.EventName))
                {
                    var action = request.ProcessManager.EventNameToProcessStepDictionary[a.Data.AggregateEvent.EventName];
                    try
                    {
                        aggregate.InitalizeProcessStep(action, a.Data.AggregateEvent);
                        await action.StepAction(a.Data.AggregateEvent);
                        aggregate.CompleteProcessStep();
                        aggregate.ChangeProcessData(request.ProcessManager.ViewBag);
                    }
                    catch (Exception ex)
                    {
                        // TODO: build in resilit logic here
                        aggregate.FailProcessStep(ex);
                    }
                }

                var commitEvents = aggregate.GetUncommitedEvents.Select(me => me.Merge(a.Data.AggregateEvent));
                await request.ProcessManager.ProcessManagerServices.EventWriter.WriteEventAsync(category, (List<Event>)commitEvents);
            });
            return new ProcessSetupCompleted();
        }

        private void DetermineProccessIdFromEvent(AggregateEvent a, out Guid pid)
        {
            if (a.ProcessId == null)
            {
                pid = a.EventId;
            }
            else
            {
                pid = a.ProcessId.Value;
            }
        }
    }
}
