﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Apis;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.eventstore.subscription.Services;
using lifebook.core.processmanager.Models;
using lifebook.core.processmanager.ProcessStates;
using lifebook.core.processmanager.Syntax;
using MediatR;

namespace lifebook.core.processmanager.Services
{
    public abstract class ProcessManager
    {
        internal readonly ProcessManagerServices ProcessManagerServices;
        internal Dictionary<string, ProcessManagerStep> EventNameToProcessStepDictionary;
        internal List<EventStoreSubscriptionHook> Subscriptions = new List<EventStoreSubscriptionHook>();

        public ProcessManager(ProcessManagerServices processManagerServices)
        {
            ProcessManagerServices = processManagerServices;
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

            await ProcessManagerServices.Mediator.Send(new SetupManager(this));
        }
    }
}
