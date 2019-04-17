using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;

namespace lifebook.apigateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json", true, true)
                       .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                       .AddJsonFile("ocelot.json")
                       .AddEnvironmentVariables();
               })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   //add your logging
               })
               .UseIIS()
               .Configure(app =>
               {
                   app.UseOcelot().Wait();
               })
               .UseStartup<Startup>()
               .Build()
               .Run();
        }
    }
}
