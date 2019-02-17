using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace lifebook.core.examples.webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
                .UseEnvironment("Development")
                .UseKestrel()
                .ConfigureKestrel((ctx, opt) => {
                    opt.Listen(IPAddress.Loopback, 5001, listOpt =>
                    {
                        listOpt.UseHttps();
                    });
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

}
