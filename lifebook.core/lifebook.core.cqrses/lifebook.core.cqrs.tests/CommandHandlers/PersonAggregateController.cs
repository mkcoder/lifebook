using System.Collections.Generic;
using lifebook.core.cqrses.Attributes;
using lifebook.core.cqrses.Domains;
using lifebook.core.cqrses.Services;

namespace lifebook.core.cqrs.tests.Aggregates
{
    [CommandHandlers]
    [EventHandlers(typeof(PersonAggregate))]
    public class PersonAggregateControllers : AggregateRoot
    {
        [CommandHandlerFor("CreatePerson")]
        public AggregateEvent CreatePerson(CreatePersonCommand createPersonCommand)
        {
            return WithAggregate<PersonAggregate, AggregateEvent>(aggregate =>
            {
                return new PersonCreated()
                {
                    FirstName = createPersonCommand.FirstName,
                    LastName = createPersonCommand.LastName,
                    Age = createPersonCommand.Age
                };
            });
        }

        [CommandHandlerFor("ChangePersonNameAndAge")]
        public List<AggregateEvent> ChangePersonNameAndAge(ChangePersonNameAndAgeCommand createPersonCommand)
        {
            var result = new List<AggregateEvent>();
            result.Add(new PersonNameChanged() { Name = createPersonCommand.Name });
            result.Add(new PersonAgeChanged() { Age = createPersonCommand.Age });
            return result;
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
    }
}
