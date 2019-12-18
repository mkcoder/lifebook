using System;
using System.Threading.Tasks;

namespace lifebook.core.services.discovery
{
    public interface IServiceRegister
    {
        Task Deregister();
        Task Register(string ipAddress, int port, string scheme = "https");
    }
}
