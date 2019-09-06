using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using lifebook.core.eventstore.extensions;
using lifebook.core.services.ServiceStartup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.cqrses
{
    public class CQRSStartup : ServiceStartup
    { 
        public override void RegisterService(IWindsorContainer windsorContainer)
        {
            base.RegisterService(windsorContainer);
            windsorContainer.InstallEventStore();
        }
    }
}
