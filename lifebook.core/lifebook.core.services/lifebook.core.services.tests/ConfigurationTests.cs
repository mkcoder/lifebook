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

        [SetUp]
        public void Setup()
        {
            container.Install(FromAssembly.InThisApplication(typeof(ConfigurationInstaller).Assembly));
            configuration = container.Resolve<interfaces.IConfiguration>();
        }

        [Test]
        public void Test1()
        {
            Assert.IsFalse(configuration.GetValue<bool>("IsProduction"));
            Assert.AreEqual("lifebook.core.services.tests", configuration.GetValue<bool>("Service"));
        }
    }
}