using System;

namespace lifebook.core.eventstore.domain.api
{
    public interface ICreateEvent<T>
    {
        T Create(string eventType, DateTime created, byte[] data, byte[] metadata);
    }
}
