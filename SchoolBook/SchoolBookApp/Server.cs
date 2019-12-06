using System;
using lifebook.core.cqrses;
using lifebook.core.services.ServiceStartup;

namespace SchoolBookApp
{
    public class Server
    {
        public static void Main(string[] args)
        {
            Hosting.Start<Startup>(new CQRSServiceResolver());
        }
    }

    public class Startup : CQRSStartup
    {
    }
}
