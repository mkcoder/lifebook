using System.Threading.Tasks;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventWriter
    {
        Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e);
    }
}
