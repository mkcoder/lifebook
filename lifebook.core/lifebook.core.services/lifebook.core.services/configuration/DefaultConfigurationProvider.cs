using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using lifebook.core.services.attribute;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace lifebook.core.services.configuration
{
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
            StackFrame[] frames = new StackTrace().GetFrames();
            var initialAssembly =
                frames
                .Select(f => new { frame = f, CanBeServiceName = CanBeServiceName(f.GetMethod().ReflectedType.FullName, t.Assembly)})
                .Where(f => f.CanBeServiceName)
                .Select(f => f.frame).Last();
            return initialAssembly.GetMethod().ReflectedType.Assembly.GetName().Name;
        }

        public static bool CanBeServiceName(string text, Assembly thisAssembly)
        {
            // check 1: make sure the text is not our assembly
            if (text == thisAssembly.FullName) return false;
            return text.Contains("lifebook");
        }

    }
}