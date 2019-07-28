using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.ioc;
using lifebook.core.eventstore.services;
using lifebook.core.eventstore.testing.framework;

namespace lifebook.core.eventstore.tests.EventStoreClientTest
{
    public class EventStoreClientTestIntaller : IInstaller
    {
        public void Install(WindsorContainer container)
        {
            container.Install(FromAssembly.InThisApplication(typeof(EventStoreClientInstaller).Assembly));
            
        }
    }
}