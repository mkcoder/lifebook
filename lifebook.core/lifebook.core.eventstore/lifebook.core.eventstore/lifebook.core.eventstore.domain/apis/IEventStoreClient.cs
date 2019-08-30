using System;
using System.Threading.Tasks;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventStoreClient
    {
        bool IsConnected { get; }
        void Connect();
        Task ConnectAsync();
        void Close();
    }
}
