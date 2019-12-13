using System;
using Castle.Windsor;
using lifebook.core.services.interfaces;
using lifebook.core.services.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace lifebook.core.services.ServiceStartup
{
    public abstract class BaseServiceStartup
    {
        public IConfiguration Configuration { get; set; }

        public abstract void RegisterService(IWindsorContainer windsorContainer);

        public virtual void AfterConfigureServices(IApplicationBuilder app, IWebHostEnvironment env) { }

        public virtual void AfterConfigureServices(IServiceCollection services) { }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            hostApplicationLifetime.ApplicationStopped.Register(() => app.DeregisterService(Configuration));

            RegisterService(app.ApplicationServices.GetService<IWindsorContainer>());            
            Configuration = app.ApplicationServices.GetService<IConfiguration>();
            app.RegisterService(Configuration);
            AfterConfigureServices(app, env);
        }

        private void appShutDown()
        {
            throw new NotImplementedException();
        }
    }
}
