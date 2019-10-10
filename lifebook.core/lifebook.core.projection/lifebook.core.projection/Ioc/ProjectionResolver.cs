using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services;
using lifebook.core.projection.Services.StreamTracker;

namespace lifebook.core.projection.Ioc
{
    public class ProjectionResolver : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IStreamTracker>().ImplementedBy<EntityStreamTracker>().LifeStyle.Transient,
                Component.For<IProjectionStore>().ImplementedBy<DatabaseProjectionStore>().LifeStyle.Transient,
                Component.For<ProjectorServices>().ImplementedBy<ProjectorServices>().LifeStyle.Transient
            );
        }
    }
}
