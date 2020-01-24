using System;
using System.Threading;
using System.Threading.Tasks;
using lifebook.core.processmanager.Models;
using lifebook.core.processmanager.Services;
using lifebook.core.processmanager.Syntax;
using MediatR;

namespace lifebook.core.processmanager.ProcessStates
{
    public class SetupManager : IRequest<ManagerSetupCompleted>
    {
        public ProcessIdentity ProcessIdentity { get; } = ProcessIdentity.GetIdentity();

        public ProcessManager ProcessManager { get; }

        public SetupManager(ProcessManager processManager)
        {
            ProcessManager = processManager;
        }
    }
}
