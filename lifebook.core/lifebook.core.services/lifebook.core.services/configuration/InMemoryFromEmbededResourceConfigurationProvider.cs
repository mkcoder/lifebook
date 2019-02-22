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
            var config = JsonToDictionary(JObject.Parse(jsonText));
            configurationBuilder.AddInMemoryCollection(config);
        }
        
        private static Dictionary<string, string> JsonToDictionary(JObject jobject)
        {
            var result = new Dictionary<string, string>();
            FlattenStructure(jobject, result);
            return result;
        }

        private static void FlattenStructure(JObject jobject, Dictionary<string, string> result)
        {
            foreach (var item in jobject)
            {
                if (item.Value is JObject)
                {
                    FlattenStructure((JObject)item.Value, result);
                }
                else if (item.Value is JArray)
                {
                    result.Add(item.Value.Path.Replace('.', ':'), string.Join(",", item.Value));
                }
                else
                {
                    result.Add(item.Value.Path.Replace('.', ':'), item.Value.ToObject<string>());
                }
            }
        }
    }
}
