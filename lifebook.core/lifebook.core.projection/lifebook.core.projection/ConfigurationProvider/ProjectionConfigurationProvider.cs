using System;
using System.Collections.Generic;
using lifebook.core.services.configuration;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.projection.ConfigurationProvider
{
    public class ProjectionConfigurationProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder configurationBuilder)
        {
            var configurations = new Dictionary<string, string>();
            configurations.Add("ProjectionConnectionString", "Server=127.0.0.1;Port=5432;Database=ProjectionDataStore;User Id=postgres;Password=mysecretpassword;");
            configurationBuilder.AddInMemoryCollection(configurations);
        }
    }
}
