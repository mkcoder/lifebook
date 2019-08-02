using System.Threading.Tasks;

namespace lifebook.core.eventstore.domain.api
{
    public interface IEventWriter
    {
        void WriteEvent(StreamCategorySpecifier streamCategorySpecifier, Event e);
        Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e);
    }
}
