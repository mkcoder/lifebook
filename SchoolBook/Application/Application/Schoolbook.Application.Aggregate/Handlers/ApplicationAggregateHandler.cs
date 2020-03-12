using System;
using System.Collections.Generic;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.Schoolbook.Application.Aggregate.Handlers.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Schoolbook.Application
{
    [CommandHandlers]
    [EventHandlers(typeof(ApplicationAggregate))]
    public class ApplicationAggregateHandler : AggregateRoot
    {
        [CommandHandlerFor("CreateApplication")]
        public AggregateEvent CreateApplication(CreateApplicationForStudentV1 command)
        {
            return new ApplicationCreatedV1
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                DOB = command.DOB,
                Address = command.Address,
                Questions = command.Questions,
                EventName = "ApplicationCreated",
            };
        }
    }
}
