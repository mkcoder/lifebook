using System;
using lifebook.core.cqrses.Domains;

namespace lifebook.core.cqrses.Services
{
    public abstract class Command : ICommand
    {
        public Guid CommandId { get; set; }
        public int CommandVersion { get; set; }
        public Guid CorrelationId { get; set; }
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
            Assertion.IsNotNull(CorrelationId);
            Assertion.IsNotDefault(CorrelationId);
            Assertion.IsNotNull(CommandName);
            Assertion.IsNotNull(CommandDate);
            Assertion.IsNotDefault(CommandDate);
        }
    }

    public static class Assertion
    {
        public static void IsNotNull(object t, string message = "Object is null")
        {
            if (t == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void IsNotDefault<T>(T t, string message = "Object is null")
        {
            if (t.Equals(default(T)))
            {
                throw new ArgumentNullException(message);
            }
        }
    }
}
