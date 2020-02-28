using lifebook.core.logging.interfaces;
using lifebook.core.services.discovery;
using lifebook.core.services.interfaces;

namespace lifebook.core.processmanager.Services
{
    public class ProcessServices
    {
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }
        public INetworkServiceLocator NetworkServiceLocator { get; }

        public ProcessServices(IConfiguration configuration, ILogger logger, INetworkServiceLocator networkServiceLocator)
        {
            Configuration = configuration;
            Logger = logger;
            NetworkServiceLocator = networkServiceLocator;
        }
    }
}
