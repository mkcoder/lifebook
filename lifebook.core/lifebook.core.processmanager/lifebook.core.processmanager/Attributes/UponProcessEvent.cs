using System;
namespace lifebook.core.processmanager.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UponProcessEvent
    {
        public string EventName { get; }

        public UponProcessEvent(string eventName)
        {
            EventName = eventName;
        }
    }
}
