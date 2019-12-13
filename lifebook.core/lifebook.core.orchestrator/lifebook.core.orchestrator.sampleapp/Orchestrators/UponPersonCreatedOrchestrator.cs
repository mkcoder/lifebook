using System;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.orchestrator.Models;
using lifebook.core.orchestrator.Services;

namespace lifebook.core.orchestrator.sampleapp.Orchestrators
{
    public class UponPersonCreatedOrchestrator : EventOrchestration
    {
        public UponPersonCreatedOrchestrator(IEventStoreSubscription eventStoreSubscriptionService) : base(eventStoreSubscriptionService)
        {
        }

        public override EventSpecifier GetEventSpecifier()
        {
            return new EventSpecifier("StudentCreated", new StreamCategorySpecifier("lifebookSchoolbookapp", "Primary", "Student"));
        }

        public override async Task Orchestrate(AggregateEvent evt)
        {
            Console.WriteLine(evt.CommandName);
            Console.WriteLine(evt.EventName);

            Assert.IsCalled();
        }
    }

    public class Assert
    {
        private static int timesCalled = 0;
        public static void IsCalled()
        {

        }
    }

    public class Times
    {
        public static int Once = 1;
    }
}
