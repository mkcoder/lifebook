using System;
using lifebook.core.cqrses;
using lifebook.core.services.ServiceStartup;

namespace Schoolbook.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Hosting.Start<CQRSStartup>(new CQRSServiceResolver());
        }
    }
}
