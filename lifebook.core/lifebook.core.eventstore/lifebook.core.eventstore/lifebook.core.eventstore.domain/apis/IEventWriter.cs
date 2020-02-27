using System.Collections.Generic;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventWriter
    {
        Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e);
        Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e, byte[] data,byte[] metadata);
        Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, List<Event> e);
    }
}
