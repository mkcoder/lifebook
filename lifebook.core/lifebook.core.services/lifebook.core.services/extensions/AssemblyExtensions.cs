using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace lifebook.core.services.extensions
{
    public static class AssemblyExtensions
    {
        public static Assembly GetRootAssembly(this Assembly assembly)
        {
            StackFrame[] frames = new StackTrace().GetFrames();
            var initialAssembly =
                frames
                .Select(f => new { frame = f, CanBeServiceName = CanBeServiceName(f.GetMethod().ReflectedType.FullName, assembly) })
                .Where(f => f.CanBeServiceName)
                .Select(f => f.frame)
                .Last();
            return initialAssembly.GetMethod().ReflectedType.Assembly;
        }


        public static bool CanBeServiceName(string text, Assembly thisAssembly)
        {
            // check 1: make sure the text is not our assembly
            if (text == thisAssembly.FullName) return false;
            return text.Contains("lifebook");
        }
    }
}
