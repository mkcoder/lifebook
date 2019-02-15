using System;
using System.IO;
using Newtonsoft.Json.Linq;
using lifebook.core.tools.converter;

namespace lifebook.core.services.configuration
{
    public class ProductionWebConfigurationProvider
    {
        JObject config;

        public ProductionWebConfigurationProvider()
        {
            var assemblyName = GetType().AssemblyQualifiedName;    
            var jsonText = GetType().Assembly.FromResourceNameToEmbededAssemblyResources($"appsettings.{assemblyName}.json");
            config = JObject.Parse(jsonText);
        }

        public virtual string this[string key]
        {
            get
            {
                key = key.Replace(":", ".");
                return config.SelectToken(key).ToString();
            }
        }
    }
}
