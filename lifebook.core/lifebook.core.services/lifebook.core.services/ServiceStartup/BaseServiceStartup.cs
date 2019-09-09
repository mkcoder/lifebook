using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using lifebook.core.logging.interfaces;
using lifebook.core.logging.services;
using lifebook.core.services.extensions;
using lifebook.core.services.interfaces;
using lifebook.core.services.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            Configuration = app.ApplicationServices.GetService<IConfiguration>();
            app.RegisterService(Configuration);
            AfterConfigureServices(app, env);
        }
    }
}