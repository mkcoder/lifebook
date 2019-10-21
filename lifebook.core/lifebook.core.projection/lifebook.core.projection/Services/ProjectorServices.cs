using Castle.Windsor;
using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.logging.interfaces;
using lifebook.core.projection.Interfaces;
using lifebook.core.services.interfaces;

namespace lifebook.core.projection.Services
{
    public class ProjectorServices
    {
        public ProjectorServices(IProjectionStore projectionStore, IStreamTracker streamTracker, IConfiguration configuration,
            IEventStoreSubscription eventStoreSubscription, ILogger logger, IWindsorContainer container)
        {
            ProjectionStore = projectionStore;
            StreamTracker = streamTracker;
            Configuration = configuration;
            EventStoreSubscription = eventStoreSubscription;
            Logger = logger;
            Container = container;
        }

        internal IProjectionStore ProjectionStore { get; }
        internal IStreamTracker StreamTracker { get; }
        internal IConfiguration Configuration { get; }
        internal IEventStoreSubscription EventStoreSubscription { get; }
        internal ILogger Logger { get; }
        internal IWindsorContainer Container { get; }
    }
}
