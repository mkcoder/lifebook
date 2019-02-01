using System;
using Xunit;

namespace lifebook.core.database.databaseprovider.test
{
    public class ConfigurationProviderTests
    {
        ConfigurationProvider cp = new ConfigurationProvider();

        [Fact]
        public void Test1()
        {
            Assert.Equal("Warning", cp["Logging.LogLevel.Default"]);        
        }
    }
}
