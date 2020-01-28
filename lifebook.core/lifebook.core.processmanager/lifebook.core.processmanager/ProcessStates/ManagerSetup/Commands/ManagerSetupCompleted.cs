using System.Threading;
using System.Threading.Tasks;
using lifebook.core.processmanager.Models;
using MediatR;

namespace lifebook.core.processmanager.ProcessStates
{
    public class ManagerSetupCompleted
    {
        public ProcessIdentity ProcessIdentity { get; }

        public ManagerSetupCompleted(ProcessIdentity processIdentity)
        {
            ProcessIdentity = processIdentity;
        }
    }
}