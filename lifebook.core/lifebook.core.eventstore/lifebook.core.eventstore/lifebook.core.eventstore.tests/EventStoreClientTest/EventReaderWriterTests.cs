using System;
using System.Text.Json;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.domain.api;
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
            await eventWriter.WriteEventAsync(streamCategory, testEvent);
            
            var e = (AggregateEvent)await eventReader.GetLastEventWrittenToStreamAsync(streamCategory);
            var myTestEvent = e.Data.TransformDataFromByte(b => JsonSerializer.Deserialize<TestEvent>(b));
            Assert.AreEqual(testEvent.EntityId, e.EntityId);
            Assert.IsNotNull(e.Data);
            Assert.AreEqual(testEvent.TestProperty, myTestEvent.TestProperty);
        }
    }
}