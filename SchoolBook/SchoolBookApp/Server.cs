using System;
using Castle.Windsor;
using lifebook.core.cqrses;
using lifebook.core.orchestrator.Runner;
using lifebook.core.projection.Hosting;
using lifebook.core.services.ServiceStartup;
using lifebook.SchoolBookApp.Projectors;

namespace lifebook.SchoolBookApp
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
        public override void RegisterService(IWindsorContainer windsorContainer)
        {
            base.RegisterService(windsorContainer);
            try
            {
                OrchestratorRunner.Run(windsorContainer);
                ProjectorHosting.Run(windsorContainer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
