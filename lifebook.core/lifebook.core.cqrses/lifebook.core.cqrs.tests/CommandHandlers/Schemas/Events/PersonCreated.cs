using lifebook.core.cqrses.Domains;

namespace lifebook.core.cqrs.tests.Aggregates
{
    public class PersonCreated : AggregateEvent
    {
        public string FirstName { get; set; }
        public int Age { get; set; }
    }
}
