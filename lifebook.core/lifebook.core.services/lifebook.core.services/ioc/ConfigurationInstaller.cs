using System;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using lifebook.core.services.configuration;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.services
{
    public class ConfigurationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                // register all our IConfigurationProviderInitializer

                Classes.FromAssembly(Assembly.GetExecutingAssembly())
                    .BasedOn<IConfigurationProviderInistalizer>()  
                    .If(t =>
                    {
                        var attributes = t.GetCustomAttributes(typeof(ProductionConfigurationAttribute)).FirstOrDefault();
                        if (attributes != null && Environment.GetEnvironmentVariable("DEV_ENV") != "PRODUCTION") return false;                        
                        return true;
                    })
                    .LifestyleTransient()
                    .WithServiceFromInterface(),
                // when we rebind it with on create and pass it through the initializer
                Component.For<IConfigurationBuilder>().OnCreate(builder =>
                {
                    var r = container.ResolveAll<IConfigurationProviderInistalizer>();
                    var provider = new ConfigurationBuilderDefaultInitalizer(r);
                    provider.configure(builder);
                }).ImplementedBy<ConfigurationBuilder>().LifestyleTransient()
            );
        }
    }
}