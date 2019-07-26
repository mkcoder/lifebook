using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.ioc;
using lifebook.core.eventstore.services;
using NSubstitute;
using NUnit.Framework;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    public sealed class EventWriterTests
    {
        WindsorContainer container = new WindsorContainer();
        IEventWriter eventWriter;
        IEventReader eventReader;
        private static Guid aggregateId = Guid.Parse("0e72cae2-a160-41aa-b168-97b420977f02");
        StreamCategorySpecifier streamCategory = new StreamCategorySpecifier("eventwriter", "test", "eventwriter", aggregateId);
        TestEvent testEvent = new TestEvent()
        {
            AggregateId = aggregateId,
            TestProperty = "Abc"
        };

        public EventWriterTests()
        {
            container.Install(FromAssembly.InThisApplication(typeof(EventStoreClientInstaller).Assembly));
            var eventStoreClient = Substitute.For<FakeEventStoreClient>();
            eventWriter = new EventWriter(eventStoreClient);
            eventReader = new EventReader(eventStoreClient);
        }

        [Test]
        public void WriteAnEventToStreamAndReadWorks()
        {
            eventWriter.WriteEvent(streamCategory, testEvent);
            
            var e = (TestEvent)eventReader.GetLastEventWrittenToStream(streamCategory);
            Assert.AreEqual(testEvent.AggregateId, e.AggregateId);
            Assert.AreEqual(testEvent.TestProperty, e.TestProperty);
        }
    }
}