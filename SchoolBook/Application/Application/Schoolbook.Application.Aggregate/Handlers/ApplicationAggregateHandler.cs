using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Schoolbook.Application
{
    [CommandHandlers]
    [EventHandlers(typeof(ApplicationAggregate))]
    public class ApplicationAggregateHandler : AggregateRoot
    {
        [HttpGet("Hello")]
        public string Hello()
        {
            return "Hello";
        }
    }
}
