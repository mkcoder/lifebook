using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.orchestrator.Interfaces;
using lifebook.core.services.extensions;

namespace lifebook.core.orchestrator.Ioc
{
    public class OrchestratorResolver : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromAssemblyInThisApplication(GetType().Assembly.GetRootAssembly())
                    .BasedOn<IOrchestrate>()
                    .LifestyleTransient()
                    .WithServiceSelf()
                    .WithServiceAllInterfaces()
            );
        }
    }
}
