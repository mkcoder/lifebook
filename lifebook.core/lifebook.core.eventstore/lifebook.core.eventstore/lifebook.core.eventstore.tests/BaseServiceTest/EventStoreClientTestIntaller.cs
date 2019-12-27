using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.subscription.Ioc;
using lifebook.core.eventstore.testing.framework;
using lifebook.core.ioc;

namespace lifebook.core.eventstore
{
    public class EventStoreClientTestIntaller : IInstaller
    {
        public virtual void Install(WindsorContainer container)
        {
            container.Install(new core.services.ServiceInstaller());
            container.Install(new EventStoreClientInstaller());
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