using System;
using lifebook.core.cqrses.Domains;

namespace lifebook.core.cqrses.Attributes
{
    public class EventHandlersAttribute : Attribute
    {
        public Type EventHandlers { get; }

        public EventHandlersAttribute(Type eventHandlerClass)
        {
            EventHandlers = eventHandlerClass;
        }
    }
}
