using System;
namespace lifebook.core.projection.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class UponEvent : Attribute
    {
        public string EventName { get; }

        public UponEvent(string eventName)
        {
            EventName = eventName;
        }
    }
}
