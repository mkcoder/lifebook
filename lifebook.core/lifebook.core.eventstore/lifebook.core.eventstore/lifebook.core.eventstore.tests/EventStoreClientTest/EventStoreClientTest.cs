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

        [SetUp]
        public void Setup()
        {
            container.Install(FromAssembly.InThisApplication(typeof(EventStoreClientInstaller).Assembly));
            _sut = container.Resolve<AbstractEventStoreClient>();
            var ew = container.Resolve<IEventWriter>();
        }

        [Test]
        public void Test()
        {

        }
    }
}