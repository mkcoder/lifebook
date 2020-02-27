using System;
using System.Collections.Generic;
using lifebook.core.services.configuration;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.processmanager.ConfigurationBuilderProvider
{
    public class DefaultProcessManagerConfiguration : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder configurationBuilder)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("ProcessManagerMode", "ProcessManager");
            configurationBuilder.AddInMemoryCollection(dict);
        }
    }
}
