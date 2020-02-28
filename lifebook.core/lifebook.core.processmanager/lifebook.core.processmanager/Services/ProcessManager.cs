using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Services;
using lifebook.core.processmanager.ProcessStates;
using lifebook.core.processmanager.ProcessStates.ProcessSetup;
using lifebook.core.processmanager.Syntax;
using Newtonsoft.Json.Linq;

namespace lifebook.core.processmanager.Services
{
    public abstract class ProcessManager
    {
        internal readonly ProcessManagerServices ProcessManagerServices;
        internal Dictionary<string, ProcessManagerStep> EventNameToProcessStepDictionary;
        internal List<EventStoreSubscriptionHook> Subscriptions = new List<EventStoreSubscriptionHook>();
        public dynamic ViewBag { get; set; } = new JObject();
        protected ProcessServices ProcessServices { get; private set; }
        protected string ThisService { get; }
        protected string ThisInstance { get; }
        protected string ThisCategory { get; } = "Process";
        protected bool RebuildSteps { get; } = false;

        public ProcessManager(ProcessManagerServices processManagerServices)
        {
            ProcessManagerServices = processManagerServices;
            ThisService = processManagerServices.ServiceName;
            ThisInstance = processManagerServices.Instance;
            ProcessServices = new ProcessServices(processManagerServices.Configuration, processManagerServices.Logger, processManagerServices.NetworkServiceLocator);
        }

        public abstract ProcessManagerConfiguration GetConfiguration();

        protected virtual async Task Start()
        {
            var configuration = GetConfiguration();
            var processSteps = configuration.GetProcessManagerSteps;

            // _mediator.Send<StartProcessManagerSetup>()

            /*
                1. Setup Manager
                    a. Check if process needs a manager
                        b. Manager mode:
                            - Subscribe to all manager stream
                            - Create a queue on rabitmq
                            - Start a process
                2. Setup Process
                    a. Subscribe to movenext on queue
             */            
            EventNameToProcessStepDictionary = processSteps
                .Select(s => new { s.EventSpecifier, StepAction = s })
                .Select(s => s.EventSpecifier.ToDictionary(e => e.EventName, e => s.StepAction))
                .SelectMany(dict => dict)
                .ToDictionary(d => d.Key, d => d.Value);

            ProcessManagerServices.SetEventNameToProcessStepMapping(EventNameToProcessStepDictionary);

            var managerSetupResult = await ProcessManagerServices.Mediator.Send(new SetupManager(this));
            var processSetupResult = await ProcessManagerServices.Mediator.Send(new SetupProcess(this, managerSetupResult.ProcessIdentity));
        }

        public async Task WriteCustomEventAsync(string eventName, Event evt, JObject data)
        {
            evt.EventName = eventName;
            evt.DateCreated = DateTime.UtcNow;
            evt.EventId = Guid.NewGuid();
            await ProcessManagerServices.EventWriter.WriteEventAsync(new StreamCategorySpecifier(ThisService, ThisInstance, ThisCategory, evt.EntityId), evt, data.ToByte(), null);
        }
    }

    public static class Extension
    {
        public static byte[] ToByte(this JToken jToken)
        {
            return Encoding.UTF8.GetBytes(jToken.ToString());
        }
    }
}
