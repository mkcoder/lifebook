using System.Threading.Tasks;

namespace lifebook.core.orchestrator.Interfaces
{
    public abstract class AbstrateOrchestrate : IOrchestrate
    {
        internal abstract Task Run();
    }
}
