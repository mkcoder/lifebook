using System;
using lifebook.core.services.ServiceStartup;

namespace lifebook.core.services.exampleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            Hosting.Start<ServiceStartup.ServiceStartup>();
        }
    }
}
