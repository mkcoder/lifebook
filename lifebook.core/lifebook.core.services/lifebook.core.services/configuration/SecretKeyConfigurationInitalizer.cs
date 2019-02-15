using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace lifebook.core.services.configuration
{
    [ProductionConfiguration]
    /// Used during production to build a key/value pair off all the settings
    public class SecretKeyConfigurationInitalizer : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder builder)
        {
            builder.AddInMemoryCollection(new Dictionary<string, string>() { { "production_secret", "true" } });
        }
    }
}
