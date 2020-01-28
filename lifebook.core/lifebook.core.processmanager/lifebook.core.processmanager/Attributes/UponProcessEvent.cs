using System;
namespace lifebook.core.processmanager.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    internal class UponProcessEvent : Attribute
    {
        public string EventName { get; }

        public UponProcessEvent(string eventName)
        {
            EventName = eventName;
        }
    }
}
