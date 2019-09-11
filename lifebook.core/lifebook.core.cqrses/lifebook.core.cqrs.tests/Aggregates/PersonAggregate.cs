using System;
using System.Text.Json;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.models;
using static lifebook.core.cqrs.tests.Aggregates.PersonAggregateController;

namespace lifebook.core.cqrs.tests.Aggregates
{
    [EventHandlers(typeof(PersonAggregate))]
    public class PersonAggregate : Aggregate
    {
        public string FirstName { get; private set; }

        [EventHandlerFor("PersonCreated")]
        public void Handle(AggregateEvent e)
        {
            var personCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<PersonCreated>(j));
            this.FirstName = personCreated.FirstName;
        }

        public PersonAggregate GetAggregate() => this;
    }
}
