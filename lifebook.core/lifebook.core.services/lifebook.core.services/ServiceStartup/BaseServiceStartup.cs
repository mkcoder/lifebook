using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using lifebook.core.logging.interfaces;
using lifebook.core.logging.services;
using lifebook.core.services.extensions;
using lifebook.core.services.interfaces;
using lifebook.core.services.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.services.ServiceStartup
{
    public abstract class BaseServiceStartup
    {
        private readonly IWindsorContainer _container = new WindsorContainer();
        public interfaces.IConfiguration Configuration { get; set; }

        public abstract void RegisterService(IWindsorContainer windsorContainer);

        public void ConfigureServices(IServiceCollection services)
        {
            _container.Install(FromAssembly.InThisApplication(GetType().Assembly.GetRootAssembly()));
            _container.Register(Component.For<ILogger>().ImplementedBy<Logger>());

            Configuration = _container.Resolve<IConfiguration>();

            RegisterService(_container);
            WindsorRegistrationHelper.CreateServiceProvider(_container, services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.RegisterService(Configuration);
        }
    }
}