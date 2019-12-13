using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.orchestrator.Interfaces;
using lifebook.core.orchestrator.Ioc;
using lifebook.core.services.extensions;

namespace lifebook.core.orchestrator.Runner
{
    public class OrchestratorRunner
    {
        private static bool Installed = false;
        private static object _lock = new object();

        public static async Task Run(IWindsorContainer container)
        {
            if(!Installed)
            {
                lock (_lock)
                {
                    Installed = true;
                    container.Install(FromAssembly.Instance(typeof(OrchestratorResolver).Assembly));
                }
            }

            var projectors = container.ResolveAll<IOrchestrate>();
            foreach (AbstrateOrchestrate projector in projectors)
            {
                await projector.Run();
            }
        }
    }
}
