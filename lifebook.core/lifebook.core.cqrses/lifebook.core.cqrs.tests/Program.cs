using lifebook.core.cqrses;
using lifebook.core.services.ServiceStartup;

namespace lifebook.core.cqrs.tests
{
    public class Program
    {
        static void Main(string[] args)
        {
            Hosting.Start<CQRSStartup>(new CQRSServiceResolver());
        }
    }
}
