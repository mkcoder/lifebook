using lifebook.core.eventstore.subscription.Interfaces;
using lifebook.core.logging.interfaces;
using lifebook.core.projection.Interfaces;
using lifebook.core.services.interfaces;

namespace lifebook.core.projection.Services
{
    public class ProjectorServices
    {
        public ProjectorServices(IProjectionStore projectionStore, IStreamTracker streamTracker, IConfiguration configuration,
            IEventStoreSubscription eventStoreSubscription, ILogger logger)
        {
            ProjectionStore = projectionStore;
            StreamTracker = streamTracker;
            Configuration = configuration;
            EventStoreSubscription = eventStoreSubscription;
            Logger = logger;
        }

        public IProjectionStore ProjectionStore { get; }
        public IStreamTracker StreamTracker { get; }
        public IConfiguration Configuration { get; }
        public IEventStoreSubscription EventStoreSubscription { get; }
        public ILogger Logger { get; }
    }
}
