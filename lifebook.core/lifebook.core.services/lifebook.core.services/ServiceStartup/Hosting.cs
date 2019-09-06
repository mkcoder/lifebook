using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace lifebook.core.services.ServiceStartup
{
    public static class Hosting
    {
        public static void Start<T>() where T : BaseServiceStartup
        {
            WebHost
                .CreateDefaultBuilder()
                .UseStartup<T>()
                .Build()
                .Run();
        }
    }
}