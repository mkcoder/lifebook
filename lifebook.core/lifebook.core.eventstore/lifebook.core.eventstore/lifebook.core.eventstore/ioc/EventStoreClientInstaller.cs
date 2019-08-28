using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.services;

namespace lifebook.core.ioc
{
    public class EventStoreClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();

            container.Register(
                Component.For<AbstractEventStoreClient>().ImplementedBy<FakeEventStoreClient>().IsFallback().LifeStyle.Transient,
                Component.For<AbstractEventStoreClient>().ImplementedBy<EventStoreClient>().LifeStyle.Transient,
                Component.For<IEventStoreClientFactory>().AsFactory(),
                Component.For<IEventStoreClient>().ImplementedBy<EventStoreClient>().Named("EventStoreClient").LifeStyle.Transient,
                Component.For<IEventWriter>().ImplementedBy<EventWriter>().LifeStyle.Transient,
                Component.For<IEventReader>().ImplementedBy<EventReader>().LifeStyle.Transient,
                Component.For<EventStoreConfiguration>().ImplementedBy<EventStoreConfiguration>().IsDefault().LifeStyle.Singleton
            );            
        }
    }
}
