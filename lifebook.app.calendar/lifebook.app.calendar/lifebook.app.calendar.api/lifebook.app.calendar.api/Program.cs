using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace lifebook.app.calendar.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
            //.UseUrls("https://0.0.0.0:5001", "http://0.0.0.0:5000")
            .UseKestrel()
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build().Run();
        }
    }
}
