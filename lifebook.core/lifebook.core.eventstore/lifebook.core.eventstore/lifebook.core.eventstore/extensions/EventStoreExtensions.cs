using System;
using Castle.Windsor;

namespace lifebook.core.eventstore.extensions
{
    public static class EventStoreIOCExtension
    {
        public static void UseEventStore(this IWindsorContainer windsor)
        {
            windsor.Install();
        }
    }
}
