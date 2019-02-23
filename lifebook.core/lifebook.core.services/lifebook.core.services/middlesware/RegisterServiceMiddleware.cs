using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using lifebook.core.services.discovery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.services.middlesware
{
    public class RegisterServiceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NetworkServiceLocator networkServiceLocator = new NetworkServiceLocator();
        readonly IConfiguration _configuration;

        public RegisterServiceMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var httpCheck1 = new AgentServiceCheck()
            {
                HTTP = $"{context.Request.Scheme}://{context.Request.Host.Host}:{context.Request.Host.Port}/consul/health",
                Interval = TimeSpan.FromSeconds(30),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5),
            };

            var httpCheck2 = new AgentServiceCheck()
            {
                HTTP = $"{context.Request.Scheme}://{context.Connection.LocalIpAddress}:{context.Connection.LocalPort}/consul/health",
                Interval = TimeSpan.FromSeconds(30),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(5),
            };

            var service = new AgentServiceRegistration()
            {
                ID = $"{_configuration["ServiceName"]}_{_configuration["ServiceHost"]}",
                Name = $"{_configuration["ServiceName"]}",
                Address = context.Request.Host.Host,
                Port = (int)context.Request.Host.Port,
                //Meta = $"{_configuration["Tags"] ?? "tag|empty"}".Split(",").Select(s => new { key = s.Split('|')[0], value = s.Split('|')[1] }).ToDictionary(kv => kv.key, kv => kv.value),
                Checks = new AgentServiceCheck[]{ httpCheck1, httpCheck2 }
            };


            if (_configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
            {
                service.Checks = null;
            }

            await networkServiceLocator.RegisterService(service);

            await _next(context);
        }
    }
}
