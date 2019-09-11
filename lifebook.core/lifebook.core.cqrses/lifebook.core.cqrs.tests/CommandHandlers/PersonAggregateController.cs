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
    }
}
