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
                await request.ProcessManager.ProcessManagerServices.SemaphoreSlim.WaitAsync();
                try
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
                            aggregate.ChangeProcessData(request.ProcessManager.ViewBag);
                            aggregate.CompleteProcessStep();
                        }
                        catch (Exception ex)
                        {
                            // TODO: build in resilit logic here
                            aggregate.FailProcessStep(ex);
                        }
                    }

                    var uncommited = aggregate.GetUncommitedEvents.Select(me => me.CommitEvent(a.Data.AggregateEvent));
                    foreach (var item in uncommited)
                    {
                        await request.ProcessManager.ProcessManagerServices.EventWriter.WriteEventAsync(category, item.ae, item.data, null);
                    }
                }
                catch (Exception ex)
                {
                    request.ProcessManager.ProcessManagerServices.Logger.Error(ex, $"Something went wrong {JObject.FromObject(a).ToString()}", a);
                }
                finally
                {
                    request.ProcessManager.ProcessManagerServices.SemaphoreSlim.Release();
                }
            });
            return new ProcessSetupCompleted();
        }

        private void DetermineProccessIdFromEvent(AggregateEvent a, out Guid pid)
        {
            if (a.ProcessId == null)
            {
                pid = a.EntityId;
                a.ProcessId = pid;
                if(a.ParentProcessId == null)
                {
                    a.ParentProcessId = pid;
                }
            }
            else
            {
                pid = a.ProcessId.Value;
            }
        }
    }
}
