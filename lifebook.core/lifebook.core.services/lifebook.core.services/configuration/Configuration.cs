using System;
using lifebook.core.services.interfaces;
using Microsoft.Extensions.Configuration;

namespace lifebook.core.services.configuration
{
    public class Configuration : interfaces.IConfiguration
    {
        private readonly IConfigurationRoot _configurationBuilder;

        public Configuration(IConfigurationBuilder configurationBuilder)
        {
            _configurationBuilder = configurationBuilder.Build();
        }

        public string GetValue(string key) => _configurationBuilder[key];
        public T GetValue<T>(string key) => (T)Convert.ChangeType(GetValue(key), typeof(T));
        public T TryGetValueOrDefault<T>(string key, T defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(GetValue(key), typeof(T));
            }
            catch (InvalidCastException)
            {
                return defaultValue;
            }
        }
        public string this[string key] => GetValue(key);
    }
}
