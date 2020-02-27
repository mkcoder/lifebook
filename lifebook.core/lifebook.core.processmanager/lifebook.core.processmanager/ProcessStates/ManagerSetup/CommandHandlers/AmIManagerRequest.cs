using System;
using System.Threading;
using System.Threading.Tasks;
using lifebook.core.services.discovery;
using lifebook.core.services.interfaces;
using lifebook.core.services.models;
using MediatR;
using Microsoft.VisualBasic;

namespace lifebook.core.processmanager.ProcessStates
{
    public class AmIManager : IRequest<bool>
    {
        public string ProcessName { get; set; }
    }

    public class AmIManagerRequest : IRequestHandler<AmIManager, bool>
    {
        private readonly INetworkServiceLocator _networkServiceLocator;
        private readonly IConfiguration _configuration;

        public AmIManagerRequest(INetworkServiceLocator networkServiceLocator,IConfiguration configuration)
        {
            _networkServiceLocator = networkServiceLocator;
            _configuration = configuration;
        }

        public async Task<bool> Handle(AmIManager request, CancellationToken cancellationToken)
        {
            var processMode = _configuration.TryGetValueOrDefault("ProcessManagerMode", "Process").ToLowerInvariant();
            var processManagerName = $"{_configuration["ServiceName"]}_{_configuration["InstanceName"]}_Manager";
            var service = await _networkServiceLocator.FindServiceByName(processManagerName);
            if (processMode == "manager")
            {
                return true;
            }
            
            if(service.ServiceName == null && processMode == "processmanager")
            {
                return true;
            }

            return false;
        }
    }
}
