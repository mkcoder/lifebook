using System;
using lifebook.core.cqrses.Domains;
using lifebook.core.processmanager.Syntax;

namespace lifebook.core.processmanager.ProcessStates
{
    public struct ProcessStateMessageDto
    {
        public AggregateEvent AggregateEvent { get; }
        public ProcessManagerStep ProcessManagerStep { get; }

        public ProcessStateMessageDto(AggregateEvent @event, ProcessManagerStep act)
        {
            AggregateEvent = @event;
            ProcessManagerStep = act;
        }
    }
}
