﻿using System;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.providers;
using lifebook.core.eventstore.services;
using lifebook.core.logging.interfaces;
using lifebook.core.services.configuration;
using lifebook.core.services.interfaces;

namespace lifebook.core.ioc
{
    public class EventStoreClientInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                if (!container.Kernel.HasComponent(typeof(TypedFactoryFacility)))
                {
                    container.AddFacility<TypedFactoryFacility>();
                }
                IConfiguration config;
                bool useFakeEventStore = true;
                try
                {
                    config = container.Resolve<IConfiguration>();
                    useFakeEventStore = config.TryGetValueOrDefault("EventStore.UseFakeEventStore", false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Using fakeevent store");
                }
                
                if(useFakeEventStore)
                {
                    if (!container.Kernel.HasComponent(typeof(IEventStoreClient)))
                    {
                        container.Register(
                            Component.For<EventStoreConfiguration>().ImplementedBy<EventStoreConfiguration>().IsDefault().LifeStyle.Transient,
                            Component.For<AbstractEventStoreClient>()
                            .ImplementedBy<FakeEventStoreClient>()
                                .OnCreate(async es => await es.ConnectAsync())
                                .OnDestroy(async es => es.Close())
                                .LifeStyle.Singleton,
                            Component.For<IEventStoreClientFactory>().AsFactory(),
                            Component.For<IEventStoreClient>()
                                .OnCreate(async es => await es.ConnectAsync())
                                .OnDestroy(async es => es.Close())
                                .ImplementedBy<FakeEventStoreClient>()
                                .Named("EventStoreClient").LifeStyle.Singleton,
                            Component.For<IEventWriter>().ImplementedBy<EventWriter>().LifeStyle.Transient,
                            Component.For<IEventReader>().ImplementedBy<EventReader>().LifeStyle.Transient
                        );
                    }
                }
                else
                {
                    if (!container.Kernel.HasComponent(typeof(IEventStoreClient)))
                    {
                        container.Register(
                            Component.For<EventStoreConfiguration>().ImplementedBy<EventStoreConfiguration>().IsDefault().LifeStyle.Transient,
                            Component.For<AbstractEventStoreClient>()
                            .ImplementedBy<EventStoreClient>()
                                .OnCreate(async es => await es.ConnectAsync())
                                .OnDestroy(async es => es.Close())
                                .LifeStyle.Singleton,
                            Component.For<IEventStoreClientFactory>().AsFactory(),
                            Component.For<IEventStoreClient>()
                                .OnCreate(async es => await es.ConnectAsync())
                                .OnDestroy(async es => es.Close())
                                .ImplementedBy<EventStoreClient>()
                                .Named("EventStoreClient").LifeStyle.Singleton,
                            Component.For<IEventWriter>().ImplementedBy<EventWriter>().LifeStyle.Transient,
                            Component.For<IEventReader>().ImplementedBy<EventReader>().LifeStyle.Transient
                        );
                    }
                }
            }
            catch (Exception)
            {
                container.Resolve<ILogger>().Error("Unable to bind event store");
            }         
        }
    }
}
