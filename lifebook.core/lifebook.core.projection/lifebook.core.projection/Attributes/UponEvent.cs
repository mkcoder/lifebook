using System;
namespace lifebook.core.projection.Attributes
{
    public class UponEvent
    {
        public string EventName { get; }

        public UponEvent(string eventName)
        {
            EventName = eventName;
        }
    }
}
