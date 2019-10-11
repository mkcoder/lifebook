using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.eventstore.subscription.Services;

namespace lifebook.core.eventstore.subscription.Ioc
{
    public class EventStoreSubsciptionResolver : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if(!container.Kernel.HasComponent(typeof(EventStoreConfiguration)))
            {
                container.Register(
                    Component.For<EventStoreConfiguration>().ImplementedBy<EventStoreConfiguration>().IsDefault().LifeStyle.Singleton
                );
            }

            container.Register(
                Component.For<IEventStoreSubscription>().ImplementedBy<EventStoreSubscriptionService>().LifestyleTransient()
            );
        }
    }
}
