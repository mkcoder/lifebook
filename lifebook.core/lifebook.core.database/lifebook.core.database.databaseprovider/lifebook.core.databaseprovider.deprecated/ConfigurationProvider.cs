using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace lifebook.core.database.databaseprovider
{
    public class ConfigurationProvider
    {
        JObject config;
        public ConfigurationProvider()
        {
            var jsonText = GetType().Assembly.GetEmbededAssemblyResources("lifebook.core.database.databaseprovider.appsettings.json");
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
