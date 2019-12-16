using Castle.Windsor;
using lifebook.core.services.interfaces;
using lifebook.core.services.ServiceStartup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.cqrses
{
    public class CQRSStartup : ServiceStartup
    {
        public override void AfterConfigureServices(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var container = app.ApplicationServices.GetService<IWindsorContainer>();
        }

        public override void RegisterService(IWindsorContainer windsorContainer)
        {
            base.RegisterService(windsorContainer);
        }
    }
}
