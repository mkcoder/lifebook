using System;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.ioc;
using lifebook.core.eventstore.services;
using lifebook.core.eventstore.testing.framework;
using NSubstitute;
using NUnit.Framework;
using Tests;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    public sealed class EventWriterTests : BaseServiceTests<EventStoreClientTestIntaller>
    {        
        private static Guid aggregateId = Guid.Parse("0e72cae2-a160-41aa-b168-97b420977f02");
        private IEventWriter eventWriter;
        private IEventReader eventReader;
        private StreamCategorySpecifier streamCategory = new StreamCategorySpecifier("eventwriter", "test", "eventwriter", aggregateId);
        private TestEvent testEvent = new TestEvent(aggregateId)
        {
            TestProperty = "Abc"
        };

        public EventWriterTests()
        {
            var eventStoreClient = Substitute.For<FakeEventStoreClient>();
            eventWriter = new EventWriter(eventStoreClient);
            eventReader = new EventReader(eventStoreClient);
        }

        [Test]
        public async Task WriteAnEventToStreamAndReadWorks()
        {
            eventWriter.WriteEvent(streamCategory, testEvent);
            
            var e = (TestEvent)await eventReader.GetLastEventWrittenToStreamAsync(streamCategory);
            Assert.AreEqual(testEvent.EntityId, e.EntityId);
            Assert.AreEqual(testEvent.TestProperty, e.TestProperty);
        }
    }
}