using System;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    internal class TestEvent : Event
    {
        public TestEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }

        public string TestProperty { get; set; }
    }
}