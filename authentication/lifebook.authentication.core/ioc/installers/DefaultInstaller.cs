using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IdentityServer4.EntityFramework.DbContexts;
using lifebook.authentication.core.dbcontext;
using lifebook.authentication.core.repository;

namespace lifebook.authentication.core.ioc.installers
{
    public class DefaultInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IdentityServerDbContext>().IsDefault().LifestyleTransient().Named("lifebook.authentication.core.IdentityServerDbContext"),
                Component.For<ApiResourcesDbContext>().IsDefault().LifestyleTransient(),
                Component.For<ApiRepository>().IsDefault().LifestyleTransient(),
                Component.For<ClientDbContext>().IsDefault().LifestyleTransient(),
                Component.For<ClientRepository>().IsDefault().LifestyleTransient()
            );
        }
    }
}
