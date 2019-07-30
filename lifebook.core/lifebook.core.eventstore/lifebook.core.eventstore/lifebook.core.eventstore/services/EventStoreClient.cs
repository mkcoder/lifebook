using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using lifebook.core.eventstore.domain.interfaces;
using lifebook.core.eventstore.extensions;

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

        internal override List<Event> ReadEvent(StreamCategorySpecifier specifier)
        {
            var slice = eventStoreConnection.ReadStreamEventsForwardAsync(specifier.GetCategoryStream(), 0, int.MaxValue, true);
            return null;
        }

        internal async Task<List<AggregateEvent>> ReadEventAsync(StreamCategorySpecifier specifier)
        {

            var slice = await eventStoreConnection.ReadStreamEventsForwardAsync(specifier.GetCategoryStream(), 0, int.MaxValue, true);
            slice.Events.Select(e => AggregateEvent.Create(e.Event.EventType, e.Event.EventNumber, e.Event.Created, e.Event.EventId, e.Event.Data, e.Event.Metadata));
            return null;
        }

        internal override void WriteEvent(StreamCategorySpecifier specifier, Event e)
        {            
            eventStoreConnection.AppendToStreamAsync(specifier.GetCategoryStream(), e.Version, new EventData(e.EventId, e.GetType().AssemblyQualifiedName, true, e.EventDataToByteArray(), e.EventMetadataToByteArray()));
        }
    }
}
