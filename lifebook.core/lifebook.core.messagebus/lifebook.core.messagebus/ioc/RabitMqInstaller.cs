using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.messagebus.Interfaces;
using lifebook.core.messagebus.Services;

namespace lifebook.core.messagebus.ioc
{
    public class RabbitMqInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMessageBroker>().ImplementedBy<RabbitMqMessageBroker>(),
                Component.For<IMessageBus>().ImplementedBy<RabbitMqMessageBus>()
            );
        }
    }
}
