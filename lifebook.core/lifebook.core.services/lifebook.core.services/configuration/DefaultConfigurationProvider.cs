using System;
using System.Collections.Generic;
using lifebook.core.services.attribute;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace lifebook.core.services.configuration
{
    [DevelopmentConfiguration]
    public class DefaultConfigurationProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder cb)
        {
            List<KeyValuePair<string, string>> defaultConfiguration = new List<KeyValuePair<string, string>>();

            defaultConfiguration.Add(new KeyValuePair<string, string>("Service", GetService(GetType())));
            defaultConfiguration.Add(new KeyValuePair<string, string>("ServiceInstance", "Primary"));
            defaultConfiguration.Add(new KeyValuePair<string, string>("IsProduction", "false"));
            defaultConfiguration.Add(new KeyValuePair<string, string>("ConsulAddress", "http://localhost:8500"));
            cb.AddInMemoryCollection(defaultConfiguration);
        }

        private static string GetService(Type t)
        {
            return t.Assembly.FullName;
        }
    }
}