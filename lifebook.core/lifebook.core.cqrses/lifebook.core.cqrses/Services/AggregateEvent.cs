using System;
namespace lifebook.core.cqrses.Services
{
    public interface IEvent
    {
    }

    public sealed class AggregateEvent : IEvent
    {
        public Guid EventId { get; private set; }
        public string EventName { get; private set; }
        public int EventVersion { get; private set; }
        public Guid AggregateId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public string CommandName { get; private set; }
        public object Data { get; private set; }

        private AggregateEvent()
        {

        }

        public static AggregateEvent CreateEvent<T>(string eventName, Command cmd, T data, int version = 1)
        {
            return new AggregateEvent()
            {
                EventId = Guid.NewGuid(),
                EventName = eventName,
                EventVersion = version,
                AggregateId = cmd.AggregateId,
                CorrelationId = cmd.CorrelationId,
                CommandName = cmd.CommandName,
                Data = data
            };
        }
    }
}
