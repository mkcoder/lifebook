using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consul;
using lifebook.core.services.discovery;
using Microsoft.AspNetCore.Http;

namespace lifebook.core.services.middlesware
{
    public class RegisterServiceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string serviceName;
        private readonly NetworkServiceLocator networkServiceLocator = new NetworkServiceLocator();

        public RegisterServiceMiddleware(RequestDelegate next, string ServiceName, string Service, string tags)
        {
            _next = next;
            serviceName = ServiceName;
            this.Service = Service;
            Tags = tags;
        }

        public string Service { get; }
        public string Tags { get; }

        public async Task InvokeAsync(HttpContext context)
        {
            CatalogRegistration service = new CatalogRegistration
            {
                Node = "DS1",
                Address = $"{context.Connection.LocalIpAddress.ToString()}:{context.Connection.RemotePort}",
                Service = new AgentService
                {
                    ID = serviceName,
                    Service = Service,
                    Tags = Tags.Split(","),
                    Address = context.Request.Host.Host,
                    Port = (int)context.Request.Host.Port,
                    Meta = new Dictionary<string, string>() { { "abc", "234"} }
                },
                Check = new AgentCheck
                {
                    Node = "DS1",
                    CheckID = "default.health.check",
                    Name = "default health check",
                    Status = HealthStatus.Passing,
                    ServiceID = serviceName,
                    ServiceName = Service
                }
            };
            await networkServiceLocator.RegisterService(service);
        }
    }
}
