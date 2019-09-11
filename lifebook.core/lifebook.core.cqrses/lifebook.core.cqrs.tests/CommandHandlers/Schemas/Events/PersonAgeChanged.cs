using lifebook.core.cqrses.Domains;

namespace lifebook.core.cqrs.tests.Aggregates
{
    internal class PersonAgeChanged : AggregateEvent
    {
        public int Age { get; set; }
    }
}