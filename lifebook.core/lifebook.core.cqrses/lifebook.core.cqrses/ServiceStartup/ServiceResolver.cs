using Castle.Windsor;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.extensions;
using lifebook.core.services.ServiceStartup;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.cqrses
{
    public class ServiceResolver : IServiceResolver
    {
        void IServiceResolver.ServiceResolver(IWindsorContainer container, IServiceCollection services)
        {
            if (!container.Kernel.HasComponent(typeof(IEventStoreClient)))
            {
                container.InstallEventStore();
            }
        }
    }
}
