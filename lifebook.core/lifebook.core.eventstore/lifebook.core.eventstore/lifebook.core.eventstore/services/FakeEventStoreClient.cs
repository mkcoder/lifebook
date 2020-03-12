using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;
using lifebook.core.eventstore.extensions;

namespace lifebook.core.eventstore.services
{
    public class FakeEventStoreClient : AbstractEventStoreClient
    {
        // Category of Category
        private static ConcurrentDictionary<string, ConcurrentDictionary<string, List<Event>>> EventStore;
        private Semaphore _lock = new Semaphore(0, 1);
        public FakeEventStoreClient()
        {
            EventStore = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<Event>>>();
        }
        public override void Connect() => _connected = true;

        public override async Task ConnectAsync()
        {
            _connected = true;
        }

        public override void Close()
        {
            _connected = false;
        }

        internal override async Task WriteEventAsync(StreamCategorySpecifier specifier, Event e, byte[] data, byte[] metadata = null)
        {
            await WriteEvent(specifier, e);
        }

        internal override async Task WriteEventAsync(StreamCategorySpecifier specifier, Event e)
        {
            await WriteEvent(specifier, e);
        }

        private async Task WriteEvent(StreamCategorySpecifier specifier, Event @e)
        {
            _lock.WaitOne(300);
            if(!EventStore.ContainsKey(specifier.GetCategoryStream()))
            {
                EventStore[specifier.GetCategoryStream()] = new ConcurrentDictionary<string, List<Event>>();
            }
            if(!EventStore[specifier.GetCategoryStream()].ContainsKey(specifier.GetCategoryStreamWithAggregateId()))
            {
                EventStore[specifier.GetCategoryStream()][specifier.GetCategoryStreamWithAggregateId()] = new List<Event>();
            }
            EventStore[specifier.GetCategoryStream()][specifier.GetCategoryStreamWithAggregateId()].Add(@e);
            _lock.Release();
            await Task.CompletedTask;
        }

        internal override async Task<List<EntityEvent>> ReadEventsAsync(StreamCategorySpecifier specifier)
        {
            var result = EventStore[specifier.GetCategoryStream()]
                    .SelectMany(p => p.Value)
                    .Select(p => new EntityEvent().Create(p.EventType, p.DateCreated, p.EventDataToByteArray(), p.EventMetadataToByteArray()))
                    .ToList();
            return result;
        }

        internal override async Task<List<EntityEvent>> ReadSingleStreamEventsAsync(StreamCategorySpecifier specifier)
        {
            var result = EventStore[specifier.GetCategoryStream()][specifier.GetCategoryStreamWithAggregateId()].Select(p => (EntityEvent)p).ToList();
            return result;
        }

        internal override async Task<List<TOut>> ReadEventsAsync<T, TOut>(StreamCategorySpecifier specifier)
        {
            var result = EventStore[specifier.GetCategoryStream()][specifier.GetCategoryStreamWithAggregateId()].Select(p => (TOut)p).ToList();
            return result;
        }

        internal override async Task<List<TOut>> ReadSingeStreamEventsAsync<T, TOut>(StreamCategorySpecifier specifier)
        {
            var result = EventStore[specifier.GetCategoryStream()][specifier.GetCategoryStreamWithAggregateId()].Select(p => (TOut)p).ToList();
            return result;
        }
    }
}
