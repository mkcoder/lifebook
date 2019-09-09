using System;
using lifebook.core.cqrses;
using lifebook.core.services.interfaces;
using lifebook.core.services.ServiceStartup;

namespace lifebook.core.cqrs.tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Hosting.Start<Startup>();
        }
    }

    public class Startup : CQRSStartup
    {
    }
}
