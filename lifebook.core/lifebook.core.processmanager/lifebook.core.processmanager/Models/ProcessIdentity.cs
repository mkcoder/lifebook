using System;
using lifebook.core.processmanager.Services;

namespace lifebook.core.processmanager.Models
{
    public struct ProcessIdentity
    {
        public readonly Guid PID { get; }
        public readonly string ProcessName { get; }
        public readonly int ProcessIntId { get; }

        public ProcessIdentity(Guid pid, string pname, int pIntId)
        {
            PID = pid;
            ProcessName = pname;
            ProcessIntId = pIntId;
        }

        internal static ProcessIdentity GetIdentity()
        {
            return new ProcessIdentity(Guid.NewGuid(), DefaultProcessNameAssigner.GetName(), new Random().Next());
        }
    }
}
