using System;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.testing.framework;
using NUnit.Framework;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    public class EventStoreConfigurationTests : BaseServiceTests<EventStoreClientTestIntaller>
    {
        private readonly EventStoreConfiguration esc;

        public EventStoreConfigurationTests()
        {
            esc = container.Resolve<EventStoreConfiguration>();
        }

        [Test]
        public void EventStoreConfiguration_Is_A_Singleton()
        {
            var esc2 = container.Resolve<EventStoreConfiguration>();
            Assert.AreSame(esc, esc2);
        }

        [Test]
        public void EventStoreConfiguration_ReturnsAllValues()
        {
            Assert.IsNotNull(esc.IpAddress);
            Assert.IsNotNull(esc.Port);
            Assert.IsNotNull(esc.UseFakeEventStore);
        }
    }
}
