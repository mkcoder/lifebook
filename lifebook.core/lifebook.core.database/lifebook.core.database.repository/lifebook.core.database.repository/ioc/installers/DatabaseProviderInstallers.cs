using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.database.repository.interfaces;
using lifebook.core.database.repository.repositories;

namespace lifebook.core.database.repository.ioc.installers
{
    public class DatabaseProviderInstallers : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<UserIdentityDbContext>(),
                Component.For<IRepository<User>>().ImplementedBy<UserIdentityRepository>().Named("UserIdentityRepository")
            );
        }
    }
}
