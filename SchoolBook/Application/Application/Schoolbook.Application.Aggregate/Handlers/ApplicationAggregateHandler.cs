using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Services;

namespace Schoolbook.Application
{
    [CommandHandlers]
    [EventHandlers(typeof(ApplicationAggregate))]
    public class ApplicationAggregateHandler : AggregateRoot
    {
        public ApplicationAggregateHandler()
        {
        }
    }
}
