﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.cqrses
{
    public class BaseStartup
    { 
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Logger");
            services.AddTransient<ILogger, MyLogger>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }

    public interface ILogger { }
    public class MyLogger : ILogger
    { }
}
