using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
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
        public static void Start<T>(IServiceResolver serviceResolver = null, Action action=null) where T : BaseServiceStartup
        {
                Host
                .CreateDefaultBuilder()
                .UseServiceProviderFactory(new CustomWindosrCastleServiceProviderFactory(serviceResolver))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .Configure((ctx, app) => {
                        
                    })
                    .UseKestrel((ctx, server) =>
                    {
                        var ipaddress = IPAddresses(Dns.GetHostName());
                        server.Listen(ipaddress, GetPort(), opt =>
                        {
                            var serviceRegister = opt.ApplicationServices.GetService<IServiceRegister>();
                            serviceRegister.Register(opt.IPEndPoint.Address.ToString(), opt.IPEndPoint.Port);
                        });
                    })
                    .UseStartup<T>();

                    action?.Invoke();
                })                
                .Build()
                .Run();
        }

        private static int GetPort()
        {
            return new Random(DateTime.Now.Second).Next(1000, IPEndPoint.MaxPort);
        }

        private static IPAddress IPAddresses(string server)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); 
            IPAddress ipAddress = null;
            foreach (var address in ipHostInfo.AddressList)
            {
                if (address.IsIPv6LinkLocal)
                    continue;
                if (address.MapToIPv4().ToString() == address.ToString())
                {
                    ipAddress = address;
                    break;
                }
            }
            return ipAddress;
        }
    }

    public interface IServiceResolver
    {
        void ServiceResolver(IWindsorContainer container, IServiceCollection services);
    }
}