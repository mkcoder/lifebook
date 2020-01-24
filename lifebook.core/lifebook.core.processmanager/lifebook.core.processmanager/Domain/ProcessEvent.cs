using System;
using lifebook.core.cqrses.Domains;

namespace lifebook.core.processmanager.Domain
{
    public class ProcessEvent : AggregateEvent
    {
        public ProcessEvent()
        {
            EventType = "ProcessEvent";
        }
    }
}
