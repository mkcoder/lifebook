using System;
using System.Threading.Tasks;
using lifebook.core.eventstore.domain.api;
using lifebook.core.eventstore.domain.models;

namespace lifebook.core.cqrses.Domains
{
    public interface ICommand
    {
        Guid CommandId { get; set; }
        int CommandVersion { get; set; }
        Task IsValid(IEventReader eventReader, StreamCategorySpecifier category);
    }
}
