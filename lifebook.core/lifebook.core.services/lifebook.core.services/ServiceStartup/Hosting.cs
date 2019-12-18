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
                            opt.UseHttps();
                            var serviceRegister = opt.ApplicationServices.GetService<IServiceRegister>();
                            serviceRegister.Register(opt.IPEndPoint.Address.ToString(), opt.IPEndPoint.Port);
                        });
                        //server.ListenAnyIP(GetPort(), opt =>
                        //{
                        //    opt.UseHttps();
                        //    var serviceRegister = opt.ApplicationServices.GetService<IServiceRegister>();
                        //    serviceRegister.Register(opt.IPEndPoint.Address.ToString(), opt.IPEndPoint.Port);
                        //});

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

        /**
      * The IPAddresses method obtains the selected server IP address information.
      * It then displays the type of address family supported by the server and its 
      * IP address in standard and byte format.
      **/
        private static IPAddress IPAddresses(string server)
        {
            IPHostEntry heserver = Dns.GetHostEntry(server);
            return heserver.AddressList[0];
        }
    }

    public interface IServiceResolver
    {
        void ServiceResolver(IWindsorContainer container, IServiceCollection services);
    }
}