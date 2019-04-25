using System;
using Castle.Facilities.AspNetCore;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using lifebook.core.examples.mvc;
using lifebook.core.services.configuration;
using lifebook.core.services.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace lifebook.core.examples.webapi
{
    public class Startup
    {

        public IConfiguration Configuration { get; set; }
        private IApplicationBuilder application { get; set; }
        private static readonly WindsorContainer Container = new WindsorContainer();

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Container.AddFacility<AspNetCoreFacility>(f => f.CrossWiresInto(services));
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            Container.Register(
                    Component.For<IConfigurationProviderInistalizer>().ImplementedBy<EmptyClass>()
                );
            Container.Install(FromAssembly.InThisApplication(GetType().Assembly));

            Configuration = Container.Resolve<IConfigurationBuilder>()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();

            return services.AddWindsor(Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            applicationLifetime.ApplicationStopping.Register(Stopping);
            applicationLifetime.ApplicationStopped.Register(Stopping);
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseHsts();
            app.RegisterService(Configuration);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            application = app;
        }

        private void Stopping()
        {
            Console.WriteLine("Stopped");
            application.DeregisterService(Configuration);
        }
    }
}
