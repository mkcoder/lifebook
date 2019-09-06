using Castle.Windsor;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace lifebook.core.services.configuration.tests
{
    public class ConfigurationTests
    {
        private WindsorContainer container = new WindsorContainer();
        private IConfigurationRoot configuration;

        [SetUp]
        public void Setup()
        {
            container.Install();            
            configuration = container.Resolve<IConfigurationBuilder>().Build();
        }

        [Test]
        public void Test1()
        {
            Assert.IsNotNull(configuration["ConsulAddress"]);
        }
    }
}