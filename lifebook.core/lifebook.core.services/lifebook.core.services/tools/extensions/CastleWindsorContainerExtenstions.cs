using System;
using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace lifebook.core.services.tools.extensions
{
    public static class CastleWindsorContainerExtenstions
    {
        public static void ResolveCoreContext<T>(this WindsorContainer container)
        {
            container.Install(FromAssembly.InThisApplication(typeof(T).Assembly));
        }
    }
}
