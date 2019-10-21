using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services;
using lifebook.core.projection.Services.ProjectionStore;
using lifebook.core.projection.Services.StreamTracker;
using lifebook.core.services.extensions;
using Microsoft.EntityFrameworkCore;

namespace lifebook.core.projection.Ioc
{
    public class ProjectionResolver : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {           
            container.Register(
                Component.For<IWindsorContainer>().Instance(container),
                Component.For<IApplicationContextCreator>().ImplementedBy<PostgresContextCreator>().LifeStyle.Singleton,
                Component.For<IApplicationContext>().ImplementedBy<ApplicationDbContext>().LifeStyle.Transient, 
                Component.For<IStreamTracker>().ImplementedBy<EntityStreamTracker>().LifeStyle.Transient,
                Component.For<IProjectionStore>().ImplementedBy<DatabaseProjectionStore>().LifeStyle.Transient,
                Component.For<ProjectorServices>().ImplementedBy<ProjectorServices>().LifeStyle.Transient,
                Component.For<DbContextOptions<PostgresContextCreator>>().Instance(new DbContextOptions<PostgresContextCreator>()),
                Component.For<DbContextOptions<ApplicationDbContext>>().Instance(new DbContextOptions<ApplicationDbContext>()),
                Classes.FromAssemblyInThisApplication(GetType().Assembly.GetRootAssembly())
                    .BasedOn<IProjector>()
                    .LifestyleTransient()
                    .WithServiceAllInterfaces()
            );
        }
    }
}
