using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.services;

namespace lifebook.core.eventstore.ioc
{
    public class EventStoreClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();

            container.Register(
                Component.For<AbstractEventStoreClient>().ImplementedBy<FakeEventStoreClient>().LifestyleTransient(),
                Component.For<IEventStoreClientFactory>().AsFactory(),
                Component.For<IEventWriter>().ImplementedBy<EventWriter>().LifestyleTransient(),
                Component.For<IEventReader>().ImplementedBy<EventReader>().LifestyleTransient()
            );
        }
    }
}
