using System;
using System.IO;
using Newtonsoft.Json.Linq;
using lifebook.core.tools.converter;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace lifebook.core.services.configuration
{
    public class ConfigurationProvider : IConfigurationProviderInistalizer
    {
        public ConfigurationProvider()
        {

        }

        public void Provide(IConfigurationBuilder configurationBuilder)
        {
            var assemblyName = GetType().AssemblyQualifiedName;
            var jsonText = GetType().Assembly.FromResourceNameToEmbededAssemblyResources($"{assemblyName}.appsettings.json");
            var config = JObject.Parse(jsonText);
            configurationBuilder.AddInMemoryCollection(config.ToObject<Dictionary<string, string>>());
        }
    }
}
