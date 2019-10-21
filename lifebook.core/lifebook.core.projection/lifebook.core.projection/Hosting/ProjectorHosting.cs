using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.cqrses.Domains;
using lifebook.core.logging.ioc;
using lifebook.core.projection.Attributes;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Services;
using lifebook.core.projection.Services.StreamTracker;
using lifebook.core.services.extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace lifebook.core.projection.Hosting
{
    public class ProjectorHosting
    {
        public static void Run(IWindsorContainer container)
        {
            var assembly = typeof(ProjectorHosting).Assembly.GetRootAssembly();
            container.Install(
                FromAssembly.InThisApplication(typeof(BootLoader).Assembly),
                FromAssembly.InThisApplication(assembly)
            );
            var projectors = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(IProjector).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();

        }
    }
}
