using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace lifebook.core.database.repository.ioc
{
    public class Bootloader : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(
                FromAssembly.Named("lifebook.core.database.databaseprovider")
            );
        }
    }
}
