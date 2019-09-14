using System;
using System.Runtime.Serialization;

namespace lifebook.core.cqrses.Services
{
    [Serializable]
    internal class UniqueConstraintViolatedException : Exception
    {
        public UniqueConstraintViolatedException()
        {
        }

        public UniqueConstraintViolatedException(string message) : base(message)
        {
        }

        public UniqueConstraintViolatedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UniqueConstraintViolatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}