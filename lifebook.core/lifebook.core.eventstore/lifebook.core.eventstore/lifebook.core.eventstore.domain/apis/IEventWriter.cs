using System.Threading.Tasks;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventWriter
    {
        Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e);
    }
}
