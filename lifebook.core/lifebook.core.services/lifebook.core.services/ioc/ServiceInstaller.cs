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

namespace lifebook.core.services
{    
    public class ServiceInstaller : IWindsorInstaller
    {
        private static bool installed;
        private static object _lock = new object();
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (!installed)
            {
                lock (_lock)
                {
                    if (!installed)
                    {
                        installed = true;
                        container.Register(
                            Component.For<IServiceRegister>().ImplementedBy<ConsulServiceRegister>(),
                            Component.For<INetworkServiceLocator>().ImplementedBy<NetworkServiceLocator>(),
                            // register all our IConfigurationProviderInitializer
                            Classes.FromAssemblyInThisApplication(GetType().Assembly.GetRootAssembly())
                                .BasedOn<IConfigurationProviderInistalizer>()
                                .If(t =>
                                {
                                    var attributes = t.GetCustomAttributes(typeof(ProductionConfigurationAttribute)).FirstOrDefault();
                                    if (attributes != null && Environment.GetEnvironmentVariable("DEV_ENV") != "PRODUCTION") return false;
                                    return true;
                                })
                                .LifestyleTransient()
                                .WithServiceAllInterfaces(),
                            // when we rebind it with on create and pass it through the initializer
                            Component.For<IConfigurationBuilder>().OnCreate(builder =>
                            {
                                var r = container.ResolveAll<IConfigurationProviderInistalizer>();
                                var provider = new ConfigurationBuilderDefaultInitalizer(r);
                                provider.configure(builder);
                            }).ImplementedBy<ConfigurationBuilder>().LifeStyle.Transient.OnlyNewServices(),
                            Component.For<interfaces.IConfiguration>().ImplementedBy<Configuration>().LifeStyle.Transient.OnlyNewServices()
                        );
                    }
                }
            }                       
        }
    }
}