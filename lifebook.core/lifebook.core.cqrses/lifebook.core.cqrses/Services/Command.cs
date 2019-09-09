using System;
using lifebook.core.cqrses.Domains;

namespace lifebook.core.cqrses.Services
{
    public abstract class Command : ICommand
    {
        public Guid CommandId { get; set; }
        public int CommandVersion { get; set; }
        public Guid CorrelationId { get; set; }
        public string AggregateType { get; set; }
        public string CommandName { get; set; }
        private DateTime _utcCommandDate;
        public DateTime CommandDate
        {
            get => _utcCommandDate;
            set => _utcCommandDate = value.ToUniversalTime();
        }
        public Guid AggregateId { get; set; }

        public virtual void IsValid()
        {
            Assertion.IsNotNull(CommandId);
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
    }
}
