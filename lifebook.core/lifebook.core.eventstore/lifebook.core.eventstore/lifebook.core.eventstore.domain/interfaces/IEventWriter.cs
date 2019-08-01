using System.Threading.Tasks;

namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEventWriter
    {
        void WriteEvent(StreamCategorySpecifier streamCategorySpecifier, Event e);
        Task WriteEventAsync(StreamCategorySpecifier streamCategorySpecifier, Event e);
    }
}
