using System;
using System.Collections;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using lifebook.core.services.discovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace lifebook.core.services.ServiceStartup
{
    public static class Hosting
    {
        public static void Start<T>(IServiceResolver serviceResolver = null) where T : BaseServiceStartup
        {
                Host
                .CreateDefaultBuilder()
                .UseServiceProviderFactory(new CustomWindosrCastleServiceProviderFactory(serviceResolver))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseKestrel(server =>
                    {
                        server.ListenAnyIP(GetPort(), opt =>
                        {
                            opt.UseHttps();
                            var serviceRegister = opt.ApplicationServices.GetService<IServiceRegister>();
                            serviceRegister.Register(opt.IPEndPoint.Address.ToString(), opt.IPEndPoint.Port);
                        });
                    })
                    .UseStartup<T>();
                })                
                .Build()
                .Run();
        }

        private static int GetPort()
        {
            return new Random(DateTime.Now.Second).Next(1000, 9999);
        }
    }

    public interface IServiceResolver
    {
        void ServiceResolver(IWindsorContainer container, IServiceCollection services);
    }
}