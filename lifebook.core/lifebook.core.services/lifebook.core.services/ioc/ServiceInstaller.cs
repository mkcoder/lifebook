using System;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.services.attribute;
using lifebook.core.services.configuration;
using Microsoft.Extensions.Configuration;
using lifebook.core.services.extensions;
using lifebook.core.services.discovery;
using lifebook.core.logging.interfaces;
using lifebook.core.logging.services;
using lifebook.core.services.LifebookContainer;

namespace lifebook.core.services
{    
    public class ServiceInstaller : BaseLifebookModuleInstaller
    {
		public override void Install(ILifebookContainer container)
		{
			container
				.Register<IServiceRegister, ConsulServiceRegister>()
				.Register<ILogger, Logger>()
				.Register<INetworkServiceLocator, NetworkServiceLocator>()
				.Register<interfaces.IConfiguration, Configuration>(Lifetime.TransientPerService);

			WindsorContainer __container = (LifebookContainerToWindsorContainerProxy)container;
			__container.Register(
				Classes.FromAssemblyInThisApplication(GetType().Assembly.GetRootAssembly())
					.BasedOn<IConfigurationProviderInistalizer>()
					.If(t =>
					{
						var attributes = t.GetCustomAttributes(typeof(ProductionConfigurationAttribute)).FirstOrDefault();
						if (attributes != null  && Environment.GetEnvironmentVariable("DEV_ENV") != "PRODUCTION")
						{
							return false;
						}
						return true;
					})
					.LifestyleTransient()
					.WithServiceAllInterfaces(),
				// when we rebind it with on create and pass it through the initializer
				Component.For<IConfigurationBuilder>().OnCreate(builder =>
				{
					var r = __container.ResolveAll<IConfigurationProviderInistalizer>();
					var provider = new ConfigurationBuilderDefaultInitalizer(r);
					provider.Configure(builder);
				}).ImplementedBy<ConfigurationBuilder>().LifeStyle.Transient.OnlyNewServices(),
				Component.For<interfaces.IConfiguration>().ImplementedBy<Configuration>().LifeStyle.Transient.OnlyNewServices()
			);
		}
	}
}