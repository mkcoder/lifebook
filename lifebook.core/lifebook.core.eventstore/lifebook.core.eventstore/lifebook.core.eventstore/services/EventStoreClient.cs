using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using lifebook.core.eventstore.domain.interfaces;

namespace lifebook.core.eventstore.services
{
    public class EventStoreClient : AbstractEventStoreClient
    {
        private IEventStoreConnection eventStoreConnection;

        public EventStoreClient()
        {
            // IEventStoreClientConnection
            // IEventStoreConfiguration
            eventStoreConnection = EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2113));
        }

        public override void Connect()
        {
            eventStoreConnection.ConnectAsync().Wait();
            _connected = true;
        }

        public override async Task ConnectAsync()
        {
            await eventStoreConnection.ConnectAsync().ConfigureAwait(false);
            _connected = true;
        }

        internal override List<IEvent> ReadEvent(StreamCategorySpecifier specifier)
        {
            throw new NotImplementedException();
        }

        internal override void WriteEvent(StreamCategorySpecifier specifier, IEvent e)
        {
            throw new NotImplementedException();
        }
    }
}
