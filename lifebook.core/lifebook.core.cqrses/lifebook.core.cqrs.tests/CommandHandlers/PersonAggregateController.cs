using System;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.models;
using Microsoft.AspNetCore.Mvc;

namespace lifebook.core.cqrs.tests.Aggregates
{
    [CommandHandlers]
    [EventHandlers(typeof(PersonAggregate))]
    public class PersonAggregateController : AggregateRoot
    {
        [CommandHandlerFor("CreatePerson")]
        public AggregateEvent CreatePerson([FromBody]CreatePersonCommand createPersonCommand)
        {
            return WithAggregate<PersonAggregate, AggregateEvent>(aggregate =>
            {
                return new PersonCreated()
                {
                    FirstName = createPersonCommand.FirstName
                };
            });
        }

        public class CreatePersonCommand : Command
        {
            public string FirstName { get; set; }
        }

        public class PersonCreated : AggregateEvent
        {
            public string FirstName { get; set; }
        }
    }
}
