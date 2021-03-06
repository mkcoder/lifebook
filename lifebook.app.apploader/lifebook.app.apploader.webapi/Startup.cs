﻿using System;
using Castle.Facilities.AspNetCore;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.app.apploader.webapi.Controllers;
using lifebook.core.services.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.app.apploader.webapi
{
    public class Startup
    {
        private static readonly WindsorContainer Container = new WindsorContainer();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            Container.Install(FromAssembly.InThisApplication(GetType().Assembly));

            return services.AddWindsor(Container, opts => opts.UseEntryAssembly(typeof(AppsController).Assembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.RegisterService(Configuration);
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
