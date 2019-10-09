using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.subscription.Ioc;
using lifebook.core.eventstore.testing.framework;
using lifebook.core.ioc;
using lifebook.core.logging.ioc;

namespace lifebook.core.eventstore
{
    public class EventStoreClientTestIntaller : IInstaller
    {
        public virtual void Install(WindsorContainer container)
        {
            container.Install(new core.services.ConfigurationInstaller());
            container.Install(new EventStoreClientInstaller());
            container.Install(FromAssembly.Instance(new BootLoader().GetType().Assembly));
        }
    }

    public class EventStoreSubscriptionTestIntaller : EventStoreClientTestIntaller
    {
        public override void Install(WindsorContainer container)
        {
            base.Install(container);
            container.Install(new EventStoreSubsciptionResolver());
        }
    }
}