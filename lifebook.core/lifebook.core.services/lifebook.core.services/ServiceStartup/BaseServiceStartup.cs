using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using lifebook.core.logging.interfaces;
using lifebook.core.logging.services;
using lifebook.core.services.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.services.ServiceStartup
{
    public abstract class BaseServiceStartup
    {
        private readonly IWindsorContainer _container = new WindsorContainer();
        public IConfiguration Configuration { get; set; }

        public abstract void RegisterService(IWindsorContainer windsorContainer);

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _container.Install(FromAssembly.InThisApplication(GetType().Assembly));

            Configuration = _container.Resolve<IConfigurationBuilder>()
                            .AddJsonFile("appsettings.json")
                            .Build();
            _container.Register(Component.For<ILogger>().ImplementedBy<Logger>());

            RegisterService(_container);
            return WindsorRegistrationHelper.CreateServiceProvider(_container, services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.RegisterService(Configuration);
        }
    }
}