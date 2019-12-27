using System;
using System.Collections.Generic;
using Castle.Windsor;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.logging.interfaces;
using lifebook.core.processmanager.Syntax;
using lifebook.core.services.discovery;
using lifebook.core.services.interfaces;
using MediatR;

namespace lifebook.core.processmanager.Services
{
    public class ProcessManagerServices
    {
        public IWindsorContainer Container { get; }
        public INetworkServiceLocator NetworkServiceLocator { get; }
        public IEventWriter EventWriter { get; }
        public IEventReader EventReader { get; }
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }
        public IEventStoreSubscription EventStoreSubscription { get; }
        public IMediator Mediator { get; }
        public Dictionary<string, ProcessManagerStep> EventNameToProcessStepDictionary { get; private set; }
        public string ServiceName { get; }
        public string Instance { get; }

        public ProcessManagerServices(IWindsorContainer container, INetworkServiceLocator networkServiceLocator,
            IEventWriter eventWriter, IEventReader eventReader, IConfiguration configuration, ILogger logger,
            IEventStoreSubscription eventStoreSubscription, IMediator mediator)
        {
            Container = container;
            NetworkServiceLocator = networkServiceLocator;
            EventWriter = eventWriter;
            EventReader = eventReader;
            Configuration = configuration;
            Logger = logger;
            EventStoreSubscription = eventStoreSubscription;
            Mediator = mediator;
            ServiceName = Configuration.GetValue("ServiceName");
            Instance = Configuration.GetValue("ServiceInstance");
        }

        internal void SetEventNameToProcessStepMapping(Dictionary<string, ProcessManagerStep> eventNameToProcessStepDictionary)
        {
            EventNameToProcessStepDictionary = eventNameToProcessStepDictionary;
        }
    }
}
