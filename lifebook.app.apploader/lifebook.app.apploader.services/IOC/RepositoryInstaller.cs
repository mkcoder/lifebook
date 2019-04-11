using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.app.apploader.services.Database;
using lifebook.app.apploader.services.Models;
using lifebook.app.apploader.services.Repository;

namespace lifebook.app.apploader.services.IOC
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<AppDbContext>().ImplementedBy<AppDbContext>().LifestyleTransient(),
                Component.For<IRepository<UserApps>>().ImplementedBy<UserAppsRepository>().LifestyleTransient(),
                Component.For<IRepository<App>>().ImplementedBy<AppsRepository>().LifestyleTransient()
            );
        }
    }
}
