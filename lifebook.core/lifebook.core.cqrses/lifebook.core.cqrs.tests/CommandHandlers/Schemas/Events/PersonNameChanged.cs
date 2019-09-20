using lifebook.core.cqrses.Domains;

namespace lifebook.core.cqrs.tests.Aggregates
{
    internal class PersonNameChanged : AggregateEvent
    {
        public string Name { get; set; }
    }
}
