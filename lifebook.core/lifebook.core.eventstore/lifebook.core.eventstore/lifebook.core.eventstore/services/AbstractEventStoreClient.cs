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

        // Internal 
        internal abstract List<IEvent> ReadEvent(StreamCategorySpecifier specifier);

        internal abstract void WriteEvent(StreamCategorySpecifier specifier, IEvent @e);
    }
}
