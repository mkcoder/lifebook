﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.logging.interfaces;
using lifebook.core.projection.ConfigurationProvider;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services;
using lifebook.core.services;
using lifebook.core.services.configuration;
using lifebook.core.services.extensions;

namespace lifebook.core.projection.Hosting
{
    public class ProjectorHosting
    {
        public static async Task Run(IWindsorContainer container)
        {
            if(!container.Kernel.HasComponent(typeof(ILogger)))
            {
                var assembly = typeof(ProjectorHosting).Assembly.GetRootAssembly();
                container.Install(
                    FromAssembly.InThisApplication(typeof(ServiceInstaller).Assembly),
                    FromAssembly.InThisApplication(assembly)
                );
            }

            var contextCreator = container.Resolve<IApplicationContextCreator>();
            contextCreator.CreateContext();
            var projectors = container.ResolveAll<IProjector>();

            foreach (var projector in projectors)
            {
                Start(projector);
            }
        }

        internal static void Start(IProjector projector)
        {
            var type = projector.GetType().GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
            try
            {
                type?.Invoke(projector, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
