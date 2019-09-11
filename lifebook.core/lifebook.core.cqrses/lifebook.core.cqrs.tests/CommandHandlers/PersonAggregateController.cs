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
        public AggregateEvent CreatePerson(CreatePersonCommand createPersonCommand)
        {
            return WithAggregate<PersonAggregate, AggregateEvent>(aggregate =>
            {
                return new PersonCreated()
                {
                    FirstName = createPersonCommand.FirstName,
                    Age = createPersonCommand.Age
                };
            });
        }

        [CommandHandlerFor("Birthday")]
        public AggregateEvent Birthday(BirthdayCommand createPersonCommand)
        {
            return WithAggregate<PersonAggregate, AggregateEvent>(aggregate =>
            {
                return new PersonAgeChanged()
                {
                    Age = aggregate.Age + 1
                };
            });
        }

        public class CreatePersonCommand : Command
        {
            public string FirstName { get; set; }
            public int Age { get; set; }
        }

        public class BirthdayCommand : Command
        {
        }

        public class PersonCreated : AggregateEvent
        {
            public string FirstName { get; set; }
            public int Age { get; set; }
        }

        public class PersonAgeChanged : AggregateEvent
        {
            public int Age { get; set; }
        }
    }
}
