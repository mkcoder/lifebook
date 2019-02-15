using System;
namespace lifebook.core.services.models
{
    public class ServiceInfo
    {
        public string Address { get; set; }
        public string Port { get; set; }
        public string ServiceName { get; set; }
        public string ServiceHealthStatus { get; set; }
        public string FullAddress => $"{Address}:{Port}";

        internal static ServiceInfo Failed()
        {
            return new ServiceInfo()
            {
                ServiceHealthStatus = "Error"
            };
        }
    }
}
