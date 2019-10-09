using System;
namespace lifebook.core.projection.Attributes
{
    public class UponEvent
    {
        private readonly string eventName;

        public UponEvent(string eventName)
        {
            this.eventName = eventName;
        }
    }
}
