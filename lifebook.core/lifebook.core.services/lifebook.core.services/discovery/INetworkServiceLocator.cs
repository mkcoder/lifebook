using System.Threading.Tasks;
using Consul;
using lifebook.core.services.models;

namespace lifebook.core.services.discovery
{
    public interface INetworkServiceLocator
    {
        Task DeregisterService(string serviceId);
        Task<ServiceInfo> FindServiceByName(string serviceName);
        Task<ServiceInfo> RegisterService(AgentServiceRegistration service);
    }
}