using System;
namespace lifebook.core.eventstore.domain.api
{
    public interface IEventStoreClientFactory
    {
        IEventStoreClient GetEventStoreClient();
    }
}
