using System;
using System.IO;
using System.Reflection;

namespace lifebook.core.tools.converter
{
    public static class Extensions
    {
        public static string FromResourceNameToEmbededAssemblyResources(this Assembly assembly, string resourceName)
        {
            var result = String.Empty;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
