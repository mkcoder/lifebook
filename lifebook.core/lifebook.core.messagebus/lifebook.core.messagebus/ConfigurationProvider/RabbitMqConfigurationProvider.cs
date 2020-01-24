using System;
using System.Collections.Generic;
using lifebook.core.services.configuration;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.messagebus.ConfigurationProvider
{
    public class RabbitMqConfigurationProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder configurationBuilder)
        {
            Dictionary<string, string> configuration = new Dictionary<string, string>();
            configuration.Add("RabbitMqHostName", "localhost");
            configuration.Add("RabbitMqPort", "5672");
            configurationBuilder.AddInMemoryCollection(configuration);
        }
    }
}
