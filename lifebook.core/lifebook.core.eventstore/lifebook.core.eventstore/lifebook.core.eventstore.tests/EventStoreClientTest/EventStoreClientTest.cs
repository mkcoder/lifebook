using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.ioc;
using lifebook.core.eventstore.services;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    public class EventStoreClientTest
    {
        private IEventStoreClient _sut;
        WindsorContainer container = new WindsorContainer();

        public EventStoreClientTest()
        {
            container.Install(FromAssembly.InThisApplication(typeof(EventStoreClientInstaller).Assembly));
            _sut = container.Resolve<AbstractEventStoreClient>();
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
            _sut.Connect();
            Assert.IsTrue(_sut.IsConnected());
        }
    }
}