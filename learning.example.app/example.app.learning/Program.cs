using Castle.Windsor;
using lifebook.core.cqrses;
using lifebook.core.services.ServiceStartup;

namespace example.app.learning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Hosting
                .Start<CustomResolver>();
        }
    }

    public class CustomResolver : CQRSStartup
    {
        public override void RegisterService(IWindsorContainer windsorContainer)
        {
            base.RegisterService(windsorContainer);
        }
    }
}
