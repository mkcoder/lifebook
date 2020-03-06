using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using lifebook.core.services.interfaces;
using lifebook.core.services.models;

namespace lifebook.core.services.discovery
{
    public class NetworkServiceLocator : INetworkServiceLocator
    {
        private ConsulClient consul = null;
        public NetworkServiceLocator(IConfiguration configuration)
        {
            consul = new ConsulClient((obj) =>
            {
                obj.Address = new Uri(configuration["ConsulAddress"]);
            });
        }

        public async Task<ServiceInfo> RegisterService(AgentServiceRegistration service)
        {
            var s = new ServiceInfo();
            s.Address = service.Address;
            s.ServiceName = service.Name;
            var result = await consul.Agent.ServiceRegister(service);
            return s;
        }

        public async Task DeregisterService(string serviceId)
        {
            await consul.Agent.ServiceDeregister(serviceId);
        }

        public async Task<ServiceInfo> FindServiceByName(string serviceName)
        {
            var httpResponse = await consul.Catalog.Service(serviceName);            
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var response = httpResponse.Response[0];
                var serviceResponse = new ServiceInfo();
                serviceResponse.Address = response.ServiceAddress;
                serviceResponse.Port = response.ServicePort + "";
                serviceResponse.ServiceName = response.ServiceName;
                return serviceResponse;
            }
            return ServiceInfo.Failed();
        }
    }
}
