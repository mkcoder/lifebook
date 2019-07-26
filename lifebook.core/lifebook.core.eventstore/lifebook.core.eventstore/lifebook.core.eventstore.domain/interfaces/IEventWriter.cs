using System;
namespace lifebook.core.eventstore.domain.interfaces
{
    public interface IEventWriter
    {
        void WriteEvent(StreamCategorySpecifier streamCategorySpecifier, IEvent e);
    }
}
