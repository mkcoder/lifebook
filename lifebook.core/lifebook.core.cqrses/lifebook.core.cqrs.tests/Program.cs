using lifebook.core.cqrses;
using lifebook.core.services.ServiceStartup;

namespace lifebook.core.cqrs.tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Hosting.Start<Startup>(new ServiceResolver());
        }
    }

    public class Startup : CQRSStartup
    {
    }
}
