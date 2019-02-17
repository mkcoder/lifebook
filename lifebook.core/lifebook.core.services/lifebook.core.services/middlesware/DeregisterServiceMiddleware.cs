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
    public class DeregisterServiceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly NetworkServiceLocator networkServiceLocator = new NetworkServiceLocator();
        readonly IConfiguration _configuration;

        public DeregisterServiceMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await networkServiceLocator.DeregisterService($"{_configuration["ServiceName"]}_{_configuration["ServiceHost"]}");
            await _next(context);
        }
    }
}
