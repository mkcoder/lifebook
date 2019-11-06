using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using lifebook.core.eventstore.configurations;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.extensions;
using Microsoft.Extensions.Configuration;
using NeoSmart.AsyncLock;

namespace lifebook.core.eventstore.services
{
    public class EventStoreClient : AbstractEventStoreClient
    {
        private IEventStoreConnection eventStoreConnection;
        private readonly EventStoreConfiguration _eventStoreConfiguration;
        public object EventVersion { get; private set; }
        private AsyncLock asyncLock = new AsyncLock();

        public EventStoreClient(EventStoreConfiguration eventStoreConfiguration)
        {
            eventStoreConnection = EventStoreConnection.Create(ConnectionSettings.Create(), new IPEndPoint(IPAddress.Parse(eventStoreConfiguration.IpAddress), eventStoreConfiguration.Port));
            ConnectAsync().Wait();
            _eventStoreConfiguration = eventStoreConfiguration;
            eventStoreConnection.Connected += EventStoreConnection_Connected;
            eventStoreConnection.Closed += EventStoreConnection_Closed;
            eventStoreConnection.ErrorOccurred += EventStoreConnection_ErrorOccurred;
        }

        private void EventStoreConnection_ErrorOccurred(object sender, ClientErrorEventArgs e)
        {
            _connected = false;
        }

        private void EventStoreConnection_Closed(object sender, ClientClosedEventArgs e)
        {
            _connected = false;
            _closed = true;
        }

        private void EventStoreConnection_Connected(object sender, ClientConnectionEventArgs e)
         {
            _connected = true;
        }

        public override void Connect()
        {
            if(!_connected)
            {
                if (!_connected)
                {
                    eventStoreConnection.ConnectAsync().Wait();
                }
            }
        }

        public override async Task ConnectAsync()
        {
            if(!_connected)
            {
                using(await asyncLock.LockAsync())
                {
                    await eventStoreConnection.ConnectAsync();
                    _connected = true;
                }
            }
        }

        public override void Close()
        {
            if(_connected)
            {
                eventStoreConnection.Close();
                _connected = false;
                _closed = true;
                
            }
        }

        internal override async Task<List<EntityEvent>> ReadSingleStreamEventsAsync(StreamCategorySpecifier specifier)
        {
            return await ReadEventsFromStreamStringAsync<EntityEvent, EntityEvent>(specifier.GetCategoryStreamWithAggregateId());
        }

        internal override async Task<List<EntityEvent>> ReadEventsAsync(StreamCategorySpecifier specifier)
        {
            return await ReadEventsFromStreamStringAsync<EntityEvent, EntityEvent>($"$ce-{specifier.GetCategoryStream()}");
        }

        internal override async Task<List<TOut>> ReadSingeStreamEventsAsync<T, TOut>(StreamCategorySpecifier specifier)
        {
            return await ReadEventsFromStreamStringAsync<T, TOut>(specifier.GetCategoryStreamWithAggregateId());
        }

        internal override async Task<List<TOut>> ReadEventsAsync<T, TOut>(StreamCategorySpecifier specifier)
        {
            return await ReadEventsFromStreamStringAsync<T, TOut>($"$ce-{specifier.GetCategoryStream()}");           
        }

        internal override async Task WriteEventAsync(StreamCategorySpecifier specifier, Event e)
        {
            if (e.EntityId == Guid.Empty && specifier.AggregateId != null)
                throw new Exception("Entity Id must be a Guid if Aggregate Id is set in the StreamCategorySpecifier.");
            await eventStoreConnection.AppendToStreamAsync(specifier.GetCategoryStreamWithAggregateId(),
            e.EventVersion == 0 ? ExpectedVersion.Any : e.EventVersion,
            new EventData(e.EventId, e.EventType, true, e.EventDataToByteArray(), e.EventMetadataToByteArray()));
        }

        private async Task<List<TOut>> ReadEventsFromStreamStringAsync<T, TOut>(string stream) where T : ICreateEvent<TOut>, new() where TOut : Event
        {
            var result = new List<TOut>();
            var reading = true;
            int index = 0;
            int readPerCycle = 200;
            do
            {
                var slice = await eventStoreConnection.ReadStreamEventsForwardAsync(stream, index * readPerCycle, readPerCycle, true);
                result.AddRange(
                    slice.Events
                    .Select(e => new T().Create(e.Event.EventType, e.Event.Created, e.Event.Data, e.Event.Metadata))
                    .ToList()
                );
                reading = !slice.IsEndOfStream;
                index++;
            } while (reading);

            return result;
        }
    }
}
