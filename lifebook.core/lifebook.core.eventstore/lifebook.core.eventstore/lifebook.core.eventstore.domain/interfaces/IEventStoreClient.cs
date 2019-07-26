using System;
namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEventStoreClient
    {
        void Connect();
        bool IsConnected();
    }
}
