using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace lifebook.core.services.configuration.tests
{
    public class ConfigurationTests
    {
        private WindsorContainer container = new WindsorContainer();
        private interfaces.IConfiguration configuration;

        public ConfigurationTests()
        {
            container.Install(FromAssembly.InThisApplication(typeof(ConfigurationInstaller).Assembly));
            configuration = container.Resolve<interfaces.IConfiguration>();
        }

        [Test]
        public void Congiuration_GetValue_ReturnCorrectValues()
        {
            Assert.IsFalse(configuration.GetValue<bool>("IsProduction"));
            Assert.AreEqual("lifebookCoreServicesTests", configuration.GetValue<string>("ServiceName"));
        }

        [Test]
        public void Congiuration_TryGetValueOrDefault_ReturnCorrectValues()
        {
            Assert.IsFalse(configuration.TryGetValueOrDefault("DoesNotExist", false));
            Assert.AreEqual(1, configuration.TryGetValueOrDefault("DoesNotExist", 1));
        }

        [Test]
        public void Congiuration_LoadsValueFromJsonFileIfPresent_TryGetValueOrDefault_ReturnCorrectValues()
        {
            Assert.IsNotNull(configuration.TryGetValueOrDefault<string>("DefaultConnectionSettings", null));
        }
    }
}