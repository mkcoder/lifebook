using System;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.services;
using lifebook.core.eventstore.testing.framework;
using NUnit.Framework;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{    
    public class EventStoreClientTests : BaseServiceTests<EventStoreClientTestIntaller>
    {
        private IEventStoreClient eventStoreClient;

        public EventStoreClientTests()
        {
            var eventStoreClientFactory = container.Resolve<IEventStoreClientFactory>();
            eventStoreClient = eventStoreClientFactory.GetEventStoreClient();
        }

        [Test]
        public void Test_EventStoreClient_IsNotNull()
        {
            Assert.IsInstanceOf<EventStoreClient>(eventStoreClient);
        }

        [Test]
        public async Task Test_EventStoreClient_IsAbleToConnectAsync()
        {
            await eventStoreClient.ConnectAsync();
            Assert.IsTrue(eventStoreClient.IsConnected);
        }

        [Test]
        public void Test_EventStoreClient_IsAbleToDisconnectAsync()
        {
            eventStoreClient.Close();
            Assert.IsFalse(eventStoreClient.IsConnected);
        }
    }
}
