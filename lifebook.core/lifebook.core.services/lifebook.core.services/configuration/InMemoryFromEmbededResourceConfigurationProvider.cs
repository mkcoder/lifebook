using System;
using System.IO;
using Newtonsoft.Json.Linq;
using lifebook.core.tools.converter;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace lifebook.core.services.configuration
{
    public class InMemoryFromEmbededResourceConfigurationProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder configurationBuilder)
        {
            var assemblyName = GetType().AssemblyQualifiedName;
            Stream stream = GetType().Assembly.GetManifestResourceStream($"appsettings.{assemblyName}.json");
            if (stream == null) return;
            var jsonText = GetType().Assembly.FromResourceNameToEmbededAssemblyResources($"appsettings.{assemblyName}.json");
            var config = JObject.Parse(jsonText);
            configurationBuilder.AddInMemoryCollection(config.ToObject<Dictionary<string, string>>());
        }
    }
}
