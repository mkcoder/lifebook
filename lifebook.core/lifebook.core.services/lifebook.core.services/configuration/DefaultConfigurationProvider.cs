using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using lifebook.core.services.attribute;
using lifebook.core.services.extensions;
using lifebook.core.tools.converter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace lifebook.core.services.configuration
{
    public class DefaultConfigurationProvider : IConfigurationProviderInistalizer
    {
        public void Provide(IConfigurationBuilder cb)
        {
            List<KeyValuePair<string, string>> defaultConfiguration = new List<KeyValuePair<string, string>>();
            var rootAssembly = GetType().Assembly.GetRootAssembly();
            defaultConfiguration.Add(new KeyValuePair<string, string>("ServiceName", GetServiceNameFromAssemblyName(rootAssembly)));
            defaultConfiguration.Add(new KeyValuePair<string, string>("ServiceInstance", "Primary"));
            defaultConfiguration.Add(new KeyValuePair<string, string>("IsProduction", "false"));
            defaultConfiguration.Add(new KeyValuePair<string, string>("ConsulAddress", "http://localhost:8500"));
            cb.AddInMemoryCollection(defaultConfiguration);
            cb.AddJsonFile("configurations.json", true);
        }

        private static string GetServiceNameFromAssemblyName(Assembly assembly)
        {
            var name = assembly.GetName().Name;
            var result = string.Join("", name.Split('.')
                        .Select(s => s.ToLower())
                        .Select(s => (s.First() + "").ToUpper() + s.Substring(1))
                        .ToArray());

            return result[0].ToString().ToLower()+result.Substring(1);
        }
    }
}