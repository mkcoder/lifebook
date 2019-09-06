using System;
using System.Collections.Generic;
using lifebook.core.services.attribute;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace lifebook.core.services.configuration
{
    [DevelopmentConfiguration]
    public class ProductionConfigurationProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder cb)
        {
            List<KeyValuePair<string, string>> defaultConfiguration = new List<KeyValuePair<string, string>>();
            defaultConfiguration.Add(new KeyValuePair<string, string>("IsProduction", "true"));
            cb.AddInMemoryCollection(defaultConfiguration);
        }
    }
}