﻿using System;
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
                Component.For<AbstractEventStoreClient>().ImplementedBy<FakeEventStoreClient>().LifeStyle.Transient,
                Component.For<IEventStoreClientFactory>().AsFactory(),
                Component.For<IEventStoreClient>().ImplementedBy<FakeEventStoreClient>().Named("FakeEventStoreClient").LifeStyle.Transient,
                Component.For<IEventStoreClient>().ImplementedBy<EventStoreClient>().Named("EventStoreClient").LifeStyle.Transient,
                Component.For<IEventWriter>().ImplementedBy<EventWriter>().LifeStyle.Transient,
                Component.For<IEventReader>().ImplementedBy<EventReader>().LifeStyle.Transient
            );
        }
    }
}
