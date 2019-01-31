using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace authentication.server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                    .AddInMemoryIdentityResources(InMemoryResources.GetInMemoryIdentityResources())
                    .AddInMemoryClients(InMemoryResources.GetInmemoryClients())
                    .AddInMemoryApiResources(InMemoryResources.GetInMemoryApiResources())
                    .AddTestUsers(InMemoryResources.GetTestUsers())

                    ;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer(); 
        }
    }
}
