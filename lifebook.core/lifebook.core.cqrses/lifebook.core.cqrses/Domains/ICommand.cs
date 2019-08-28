using System;
namespace lifebook.core.cqrses.Domains
{
    public interface ICommand
    {
        Guid CommandId { get; set; }
        int CommandVersion { get; set; }
        void IsValid();
    }
}
