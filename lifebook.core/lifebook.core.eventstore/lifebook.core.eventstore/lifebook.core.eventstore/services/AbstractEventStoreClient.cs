using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.interfaces;

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
        internal abstract List<Event> ReadEvent(StreamCategorySpecifier specifier);

        internal abstract Task<List<AggregateEvent>> ReadEventsAsync(StreamCategorySpecifier specifier);

        [Obsolete]      
        internal abstract void WriteEvent(StreamCategorySpecifier specifier, Event @e);

        internal abstract Task WriteEventAsync(StreamCategorySpecifier specifier, Event @e);        
    }
}
