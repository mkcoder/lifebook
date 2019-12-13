using System;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Utils;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.orchestrator.Models;
using lifebook.core.orchestrator.Services;
using Newtonsoft.Json.Linq;

namespace lifebook.SchoolBookApp.Orchestrators
{
    public class UponClassroomCreatedOrchestrator : EventOrchestration
    {
        public UponClassroomCreatedOrchestrator(IEventStoreSubscription eventStoreSubscriptionService) : base(eventStoreSubscriptionService)
        {
        }

        public override EventSpecifier GetEventSpecifier()
        {
            return new EventSpecifier("ClassroomCreated", new StreamCategorySpecifier("lifebookSchoolbookapp", "Primary", "Classroom"));
        }

        public override async Task Orchestrate(AggregateEvent aggregateEvent)
        {
            var result = await CommandSenderSyntax
                .WithCommandName("CreateGradebook")
                .ToService("lifebookSchoolBookapp")
                .ToInstance("Primary")
                .WithCommandData(new JObject())
                .Send<JObject>();
            Console.WriteLine(result);
        }
    }
}
