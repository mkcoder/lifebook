using System;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.ioc;

namespace lifebook.core.eventstore.extensions
{
    public static class EventStoreIOCExtension
    {
        public static void InstallEventStore(this IWindsorContainer windsor)
        {
            windsor.Install(FromAssembly.InThisApplication(typeof(EventStoreClientInstaller).Assembly));
        }
    }
}
