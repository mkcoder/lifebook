using System;
using System.Threading.Tasks;
using Consul;
using lifebook.core.logging.interfaces;
using lifebook.core.services.interfaces;

namespace lifebook.core.services.discovery
{
    public class ConsulServiceRegister : IServiceRegister
    {
        private readonly INetworkServiceLocator _networkServiceLocator;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ConsulServiceRegister(INetworkServiceLocator networkServiceLocator, IConfiguration configuration, ILogger logger)
        {
            _networkServiceLocator = networkServiceLocator;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Register(string ipAddress, int port, string scheme = "https")
        {
            if (ipAddress.Contains(":"))
            {
                ipAddress = "localhost";
            }

            if (ipAddress.Contains("0.0.0.0"))
            {
                ipAddress = "localhost";
            }

            var httpCheck1 = new AgentServiceCheck()
            {
                HTTP = $"{scheme}://{ipAddress}:{port}/consul/health",
                Interval = TimeSpan.FromSeconds(30),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5),
            };

            var httpCheck2 = new AgentServiceCheck()
            {
                HTTP = $"{scheme}://{ipAddress}:{port}/consul/health",
                Interval = TimeSpan.FromSeconds(30),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5),
            };

            var service = new AgentServiceRegistration()
            {
                ID = $"{_configuration["ServiceName"]}_{_configuration["ServiceInstance"]}",
                Name = $"{_configuration["ServiceName"]}",
                Address = ipAddress,
                Port = port,
                //Meta = $"{_configuration["Tags"] ?? "tag|empty"}".Split(",").Select(s => new { key = s.Split('|')[0], value = s.Split('|')[1] }).ToDictionary(kv => kv.key, kv => kv.value),
                Checks = new AgentServiceCheck[] { httpCheck1, httpCheck2 }
            };


            if (!_configuration.TryGetValueOrDefault<bool>("IsProduction", false))
            {
                service.Checks = null;
            }

            try
            {                
                _networkServiceLocator.RegisterService(service).Wait();
                _logger.Information($"Service registered to consul with @Service");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "There was an error.");
            }
        }

        public async Task Deregister()
        {
            _networkServiceLocator.DeregisterService($"{_configuration["ServiceName"]}_{_configuration["ServiceHost"]}").Wait();            
        }
    }
}
