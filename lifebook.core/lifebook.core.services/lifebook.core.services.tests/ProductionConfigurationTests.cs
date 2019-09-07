using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace lifebook.core.services.configuration.tests
{
    public class ProductionConfigurationTests
    {
        private WindsorContainer container = new WindsorContainer();
        private interfaces.IConfiguration configuration;
        
        public ProductionConfigurationTests()
        {
            Environment.SetEnvironmentVariable("DEV_ENV", "PRODUCTION");
            container.Install(FromAssembly.InThisApplication(typeof(ConfigurationInstaller).Assembly));
            configuration = container.Resolve<interfaces.IConfiguration>();
        }

        [Test]
        public void Congiuration_GetValue_ReturnCorrectValues()
        {
            Assert.IsTrue(configuration.GetValue<bool>("IsProduction"));
        }

        [TearDown]
        public void After()
        {
            Environment.SetEnvironmentVariable("DEV_ENV", "");
        }
    }
}