using lifebook.core.services.ServiceStartup;

namespace lifebook.core.cqrses
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Hosting.Start<CQRSStartup>();
        }
    }
}
