﻿using System;
using Castle.Windsor;
using lifebook.core.logging.interfaces;
using lifebook.core.services.discovery;
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
        {
            var logger = app.ApplicationServices.GetService<ILogger>();
            logger.Information($"Sarting application with name: {env.ApplicationName}");
            logger.Information($"On environment: {env.EnvironmentName}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            RegisterService(app.ApplicationServices.GetService<IWindsorContainer>());
            Configuration = app.ApplicationServices.GetService<IConfiguration>();
            var serviceRegister = app.ApplicationServices.GetService<IServiceRegister>();
            logger.Information($"Registering application to consul.");

            hostApplicationLifetime.ApplicationStarted.Register(() => app.DeregisterService(Configuration));
            hostApplicationLifetime.ApplicationStopped.Register(() => serviceRegister.Deregister());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            AfterConfigureServices(app, env);
        }
    }
}
