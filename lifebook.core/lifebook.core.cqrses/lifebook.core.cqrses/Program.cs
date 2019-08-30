using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using lifebook.core.eventstore.extensions;
using lifebook.core.logging.services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.cqrses
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Hosting.Start<BaseServiceStartup>();
        }
    }

    public abstract class ServiceStartup
    {
        private readonly IWindsorContainer _container = new WindsorContainer();

        public abstract void RegisterService(IWindsorContainer windsorContainer);

        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Logger");
            services.AddTransient<ILogger, MyLogger>();
            RegisterService(_container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }              
    }

    public sealed class BaseServiceStartup : ServiceStartup
    {
        public override void RegisterService(IWindsorContainer windsorContainer)
        {
        }
    }

    public sealed class CQRSServiceStartup : ServiceStartup
    {
        public override void RegisterService(IWindsorContainer windsorContainer)
        {
            windsorContainer.InstallCQRS();
            windsorContainer.UseEventStore();
        }
    }

    public static class Hosting
    {
        public static void Start<T>() where T : ServiceStartup
        {
            WebHost
                .CreateDefaultBuilder()
                .UseStartup<T>()
                .Build()
                .Run();
        }
    }


    public static class CQRSIntaller
    {
        public static IWindsorContainer InstallCQRS(this IWindsorContainer container)
        {
            container.Register(
                Component.For<ILogger>().ImplementedBy<MyLogger>()
            );
            return container;
        }
    }
}
