using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.testing.framework;
using lifebook.core.ioc;

namespace lifebook.core.eventstore
{
    public class EventStoreClientTestIntaller : IInstaller
    {
        public void Install(WindsorContainer container)
        {
            container.Install(new core.services.ConfigurationInstaller());
            container.Install(new EventStoreClientInstaller());
        }
    }
}