using lifebook.core.eventstore;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.extensions;
using lifebook.core.eventstore.services;
using lifebook.core.eventstore.testing.framework;
using lifebook.core.eventstore.tests.EventStoreClientTest;
using NUnit.Framework;

namespace Tests
{    
    public class EventStoreClientTest : BaseServiceTests<EventStoreClientTestIntaller>
    {
        private IEventStoreClient _sut;

        public EventStoreClientTest()
        {
            _sut = container.Resolve<IEventStoreClientFactory>().GetFakeEventStoreClient();
        }

        [Test]
        public void EventStoreClient_Is_IEvenStoreClient()
        {
            Assert.IsInstanceOf<IEventStoreClient>(_sut);
            Assert.IsInstanceOf<FakeEventStoreClient>(_sut);
        }

        [Test]
        public void EventStoreClient_CanBeCretedFromIOC()
        {
            Assert.IsNotNull(_sut);
        }

        [Test]
        public void EventStoreClient_Can_Connect()
        {
            _sut.ConnectAsync();
            Assert.IsTrue(_sut.IsConnected);
        }
    }
}