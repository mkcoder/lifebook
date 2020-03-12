using Castle.Windsor;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.extensions;
using lifebook.core.ioc;
using lifebook.core.services.LifebookContainer;
using lifebook.core.services.ServiceStartup;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.cqrses
{
    public class CQRSServiceResolver : IServiceResolver
    {
        public virtual void ServiceResolver(ILifebookContainer container)
        {
            new EventStoreClientInstaller();
        }
    }
}
