using System;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace lifebook.core.services.ServiceStartup
{
    public static class Hosting
    {
        public static void Start<T>() where T : BaseServiceStartup
        {
                Host
                .CreateDefaultBuilder()
                .UseServiceProviderFactory(new CustomWindosrCastleServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<T>();
                })
                .Build()
                .Run();
        }

        private class ContainerBuilder
        {
            public ContainerBuilder()
            {
            }
        }
    }
}