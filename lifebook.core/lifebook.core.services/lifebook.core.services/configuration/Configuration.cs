using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using lifebook.core.services.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json.Linq;

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

		public Dictionary<string, string> GetAll()
		{
			var r = _configurationBuilder.Providers.Select(p => {
				JToken result = new JArray();
				var privateField = p.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).SingleOrDefault(f => f.Name.Contains("Data")) ??
									p.GetType().BaseType.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).SingleOrDefault(f => f.Name.Contains("Data"));
				var values = privateField.GetValue(p);
				result = JObject.FromObject(values);
				return result;
			}).SelectMany(t => t).ToDictionary(k => ((JProperty)k).Name, k => ((JProperty)k).Value.ToString());
			return r;
		}

		public string this[string key] => GetValue(key);
    }
}
