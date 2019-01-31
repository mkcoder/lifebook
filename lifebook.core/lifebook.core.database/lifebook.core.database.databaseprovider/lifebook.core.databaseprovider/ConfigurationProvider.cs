using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.database.databaseprovider
{
    public class ConfigurationProvider
    {
        IConfigurationRoot config;

        public ConfigurationProvider()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());
            configuration.AddJsonFile("appsettings.json");
            config = configuration.Build();
        }

        public virtual string this[string key]
        {
            get
            {
                return config[key];
            }
        }
    }
}
