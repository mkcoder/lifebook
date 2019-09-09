using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace lifebook.core.services.extensions
{
    public static class AssemblyExtensions
    {
        private static Assembly RootAssembly = null;
        private static object _lock = new object();

        public static Assembly GetRootAssembly(this Assembly assembly)
        {
            if (RootAssembly != null) return RootAssembly;
            StackFrame[] frames = new StackTrace().GetFrames();            
			var initialAssembly =
				frames
				.Select(f => new { frame = f, CanBeServiceName = CanBeServiceName(f.GetMethod().ReflectedType.FullName, assembly) })
				.Where(f => f.CanBeServiceName)
				.LastOrDefault();
            var rootAssembly = initialAssembly?.frame?.GetMethod().ReflectedType.Assembly;
            if (RootAssembly == null)
            {
                lock(_lock)
                {
                    RootAssembly = rootAssembly;
                }
            }
			return  rootAssembly ?? assembly;
        }


        public static bool CanBeServiceName(string text, Assembly thisAssembly)
        {
            // check 1: make sure the text is not our assembly
            if (text == thisAssembly.FullName) return false;
            return text.Contains("lifebook");
        }
    }
}
