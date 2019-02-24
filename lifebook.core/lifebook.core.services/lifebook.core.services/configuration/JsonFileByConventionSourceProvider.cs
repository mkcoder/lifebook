using System;
using lifebook.core.services.interfaces;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.services.configuration
{
    public class JsonFileByConventionSourceProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder configurationBuilder)
        {
            var assemblyName = GetType().Assembly.GetName().Name;
            configurationBuilder.AddJsonFile($"appsettings.{assemblyName}.json", true);
        }
    }
}
