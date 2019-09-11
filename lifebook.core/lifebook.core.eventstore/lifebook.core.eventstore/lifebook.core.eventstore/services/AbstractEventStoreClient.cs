using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.eventstore.services
{
    public abstract class AbstractEventStoreClient : IEventStoreClient
    {
        protected bool _connected;
        public bool IsConnected => _connected;

        // Public Abstract
        public abstract void Connect();

        public abstract Task ConnectAsync();

        public abstract void Close();

        // Internal 
        internal abstract Task<List<EntityEvent>> ReadEventsAsync(StreamCategorySpecifier specifier);
        internal abstract Task<List<TOut>> ReadEventsAsync<T, TOut>(StreamCategorySpecifier specifier) where T : ICreateEvent<TOut>, new() where TOut : Event;
        internal abstract Task WriteEventAsync(StreamCategorySpecifier specifier, Event @e);        
    }
}
