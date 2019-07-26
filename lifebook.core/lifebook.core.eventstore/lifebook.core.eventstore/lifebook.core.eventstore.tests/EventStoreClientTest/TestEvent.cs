using System;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    internal class TestEvent : IEvent
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; } = Guid.NewGuid();
        public string TestProperty { get; set; }
    }
}