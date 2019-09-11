using System;
using lifebook.core.cqrses.Domains;

namespace lifebook.core.cqrses.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventHandlerForAttribute : Attribute
    {
        public string EventName { get; }

        public EventHandlerForAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}
