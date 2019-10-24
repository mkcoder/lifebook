using System.Reflection;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.logging.ioc;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services;
using lifebook.core.services.extensions;

namespace lifebook.core.projection.Hosting
{
    public class ProjectorHosting
    {
        public static async Task Run(IWindsorContainer container)
        {
            var assembly = typeof(ProjectorHosting).Assembly.GetRootAssembly();

            container.Install(
                FromAssembly.InThisApplication(typeof(BootLoader).Assembly),
                FromAssembly.InThisApplication(assembly)
            );

            var contextCreator = container.Resolve<IApplicationContextCreator>();
            contextCreator.CreateContext();
            var projectors = container.ResolveAll<IProjector>();

            foreach (var projector in projectors)
            {
                Start(projector);
            }
        }

        public static void Start(IProjector projector)
        {
            var type = projector.GetType().GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
            type?.Invoke(projector, null);
        }
    }
}
