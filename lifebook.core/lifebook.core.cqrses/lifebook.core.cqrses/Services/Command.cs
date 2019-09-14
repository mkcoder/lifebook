using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lifebook.core.cqrses.Domains;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.cqrses.Services
{
    public abstract class Command : ICommand
    {
        public Guid CommandId { get; set; }
        public int CommandVersion { get; set; }
        private Guid _correlationId;
        public Guid CorrelationId
        {
            get
            {
                if (_correlationId == Guid.Empty) return Guid.NewGuid();
                return _correlationId;
            }
            set
            {
                if (value == Guid.Empty) _correlationId = Guid.NewGuid();
                else _correlationId = value;
            }
        }
        public string AggregateType { get; set; }
        public string CommandName { get; set; }
        private DateTime _utcCommandDate;
        public DateTime CommandDate
        {
            get => _utcCommandDate;
            set => _utcCommandDate = value.ToUniversalTime();
        }
        public Guid AggregateId { get; set; }

        public virtual async Task IsValid(IEventReader eventReader, StreamCategorySpecifier category)
        {
            var events = await eventReader.ReadAllEventsFromSingleStreamCategoryAsync<AggregateEventCreator, AggregateEvent>(category);
            Assertion.IsNotNull(CommandId);
            Assertion.IsCommandUnique(events, CommandId);
            Assertion.IsNotDefault(CommandId);
            Assertion.IsNotNull(AggregateId);
            Assertion.IsNotDefault(AggregateId);
            Assertion.IsNotNull(CommandName);
            Assertion.IsNotNull(AggregateType);            
        }
    }

    public static class Assertion
    {
        public static void IsNotNull(object t, string message = null)
        {
            if (t == null)
            {
                throw new ArgumentNullException(message ?? $"{nameof(t)} is null");
            }
        }

        public static void IsNotDefault<T>(T t, string message = null)
        {
            if (t.Equals(default(T)))
            {
                throw new ArgumentNullException(message ?? $"{nameof(t)} is null");
            }
        }

        internal static void IsCommandUnique(List<AggregateEvent> events, Guid commandId)
        {
            if (events.Where(e => e.CausationId == commandId).Any())
            {
                throw new UniqueConstraintViolatedException($"Unique constraint violated. CommandId ({commandId}) must be unique.");
            }
        }
    }
}
