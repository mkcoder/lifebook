using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.services;

namespace lifebook.core.eventstore.ioc
{
    public class EventStoreClientInstaller : IComponentsInstaller
    {
        public void SetUp(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();

            container.Register(
                Component.For<AbstractEventStoreClient>().ImplementedBy<EventStoreClient>().LifestyleTransient(),
                Component.For<IEventStoreClientFactory>().AsFactory(),
                Component.For<IEventWriter>().ImplementedBy<EventWriter>().LifestyleTransient()
            );
        }
    }
}
