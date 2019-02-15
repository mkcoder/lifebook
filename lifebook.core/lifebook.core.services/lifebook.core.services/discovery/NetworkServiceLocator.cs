using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using lifebook.core.services.models;

namespace lifebook.core.services.discovery
{
    public class NetworkServiceLocator
    {
        private ConsulClient consul = null;
        public NetworkServiceLocator()
        {
            consul = new ConsulClient();
        }

        public async Task<ServiceInfo> RegisterService(CatalogRegistration service)
        {
            var s = new ServiceInfo();
            s.Address = service.Service.Address;
            s.ServiceName = service.Service.Service;
            var result = await consul.Catalog.Register(service);
            return s;
        }

        public async Task<ServiceInfo> FindServiceByName(string serviceName)
        {
            var response = await consul.KV.Get(serviceName);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var serviceResponse = new ServiceInfo();
                var value = Encoding.UTF8.GetString(response.Response.Value);
                serviceResponse.Address = value.Split(":")[0];
                serviceResponse.Port = value.Split(":")[1];
                serviceResponse.ServiceName = response.Response.Key;
                return serviceResponse;
            }
            return ServiceInfo.Failed();
        }
    }
}
