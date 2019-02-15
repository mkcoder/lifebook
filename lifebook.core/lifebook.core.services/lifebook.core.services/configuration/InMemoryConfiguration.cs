using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace lifebook.core.services.configuration
{
    public class InMemoryConfiguration : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder builder)
        {
            builder.AddInMemoryCollection(new Dictionary<string, string>() { { "dev", "true" } });
        }
    }
}
