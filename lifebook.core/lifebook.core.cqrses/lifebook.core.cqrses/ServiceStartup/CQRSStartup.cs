using Castle.Windsor;
using lifebook.core.eventstore.extensions;
using lifebook.core.services.interfaces;
using lifebook.core.services.ServiceStartup;

namespace lifebook.core.cqrses
{
    public class CQRSStartup : ServiceStartup
    {
        public override void RegisterService(IWindsorContainer windsorContainer)
        {
            base.RegisterService(windsorContainer);
        }
    }
}
