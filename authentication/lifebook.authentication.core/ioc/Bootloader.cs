using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.authentication.core.ioc.installers;

namespace lifebook.authentication.core.ioc
{
    public class Bootloader : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        { 
        }
    }
}
