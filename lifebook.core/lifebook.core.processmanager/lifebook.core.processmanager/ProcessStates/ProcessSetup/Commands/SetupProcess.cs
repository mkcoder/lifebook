using System;
using lifebook.core.processmanager.Models;
using lifebook.core.processmanager.Services;
using MediatR;

namespace lifebook.core.processmanager.ProcessStates.ProcessSetup
{
    public class SetupProcess : IRequest<ProcessSetupCompleted>
    {
        public ProcessIdentity ProcessIdentity { get; }

        public ProcessManager ProcessManager { get; }

        public SetupProcess(ProcessManager processManager, ProcessIdentity processIdentity)
        {
            ProcessManager = processManager;
            ProcessIdentity = processIdentity;
        }
    }
}
