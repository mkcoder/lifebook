using System;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    internal class TestEvent : Event
    {
        public TestEvent()
        {

        }

        public TestEvent(Guid aggregateId)
        {
            EntityId = aggregateId;
        }

        public string TestProperty { get; set; }
    }
}