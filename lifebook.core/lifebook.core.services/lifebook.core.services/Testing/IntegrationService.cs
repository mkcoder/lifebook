using System;
using lifebook.core.services.LifebookContainer;

namespace lifebook.core.services.Testing
{
    internal class IntegrationService
    {
        public static ILifebookContainer Container { get; private set; } = null;
        public static bool Started { get; private set; } = false;
        private static object _lock = new object();
        public static readonly string ENV = "STARTFROMINTEGRATIONSERVER";

        private IntegrationService()
        {
        }

        public static void Instance(ILifebookContainer container)
        {
            lock (_lock)
            {
                Container = container;
            }
        }

        internal static void AppStarted()
        {
            lock (_lock)
            {
                Started = true;               
            }
        }
    }
}
