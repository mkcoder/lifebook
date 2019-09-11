using System;
using System.Text.Json;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;
using lifebook.core.eventstore.domain.models;
using static lifebook.core.cqrs.tests.Aggregates.PersonAggregateController;

namespace lifebook.core.cqrs.tests.Aggregates
{
    public class PersonAggregate : Aggregate
    {
        public string FirstName { get; private set; }
        public int Age { get; private set; }

        [EventHandlerFor("PersonCreated")]
        public void PersonCreated(AggregateEvent e)
        {
            var personCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<PersonCreated>(j));
            FirstName = personCreated.FirstName;
            Age = personCreated.Age;
        }

        [EventHandlerFor("PersonAgeChanged")]
        public void PersonAgeChanged(AggregateEvent e)
        {
            var personCreated = e.Data.TransformDataFromString(j => JsonSerializer.Deserialize<PersonAgeChanged>(j));
            Age = personCreated.Age;
        }

        public PersonAggregate GetAggregate() => this;
    }
}
