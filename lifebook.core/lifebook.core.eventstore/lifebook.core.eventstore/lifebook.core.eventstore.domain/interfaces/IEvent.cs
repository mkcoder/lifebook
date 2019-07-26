using System;
namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEvent
    {
        Guid AggregateId { get; set; }
        Guid EventId { get; set; }
    }
}
