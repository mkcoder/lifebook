using System;
using lifebook.core.processmanager.Models;
using lifebook.core.processmanager.Services;
using MediatR;

namespace lifebook.core.processmanager.ProcessStates
{
    public class RequestSetupManagerEventToEventStore : IRequest
    {
        public ProcessManager ProcessManager { get; }
        public ProcessIdentity PprocessIdentity { get; }

        public RequestSetupManagerEventToEventStore(ProcessManager processManager, ProcessIdentity processIdentity)
        {
            ProcessManager = processManager;
            PprocessIdentity = processIdentity;
        }
    }
}
