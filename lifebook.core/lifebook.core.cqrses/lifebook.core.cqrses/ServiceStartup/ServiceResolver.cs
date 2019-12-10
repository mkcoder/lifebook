using Castle.Windsor;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.extensions;
using lifebook.core.services.ServiceStartup;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.cqrses
{
    public class CQRSServiceResolver : IServiceResolver
    {
        public void ServiceResolver(IWindsorContainer container, IServiceCollection services)
        {
            if (!container.Kernel.HasComponent(typeof(IEventStoreClient)))
            {
                container.InstallEventStore();
            }
        }
    }
}
