using System;
using System.Threading.Tasks;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventStoreClient
    {
        void Connect();
        Task ConnectAsync();
        bool IsConnected { get; }

        void Close();
    }
}
