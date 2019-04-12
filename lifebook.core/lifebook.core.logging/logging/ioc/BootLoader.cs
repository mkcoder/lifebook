using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.logging.interfaces;
using lifebook.core.logging.services;

namespace lifebook.core.logging.ioc
{
    public class BootLoader : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<ILogger>().ImplementedBy<Logger>().LifestyleTransient()
            );
        }
    }
}
