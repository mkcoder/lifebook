using System;
using System.Net;
using Castle.Windsor;
using lifebook.core.logging.interfaces;
using lifebook.core.services.discovery;
using lifebook.core.services.interfaces;
using lifebook.core.services.LifebookContainer;
using lifebook.core.services.middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;

namespace lifebook.core.services.ServiceStartup
{
	public class BaseServiceStartup
	{
		public IConfiguration Configuration { get; private set; }
		public virtual void ServiceStarted(IApplicationBuilder app, ILifebookContainer container)  {}
		public virtual void ServiceStopped(IApplicationBuilder app, ILifebookContainer container)  {}
		public virtual void BeforeAppConfiguration(IApplicationBuilder app) { }
		public virtual void AfterAppConfiguration(IApplicationBuilder app, IConfiguration configuration) { }
		public virtual void BeforeConfigureServicesSetup(IServiceCollection services) { }
		public virtual void AfterConfigureServicesSetup(IServiceCollection services) { }

		public void ConfigureServices(IServiceCollection services)
		{
			BeforeConfigureServicesSetup(services);

			// our here
			services
					.AddControllers()
					.AddNewtonsoftJson();
			services
					.AddAuthorization();

			AfterConfigureServicesSetup(services);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime)
		{
			var logger = app.ApplicationServices.GetService<ILogger>();
			var container = app.ApplicationServices.GetService<ILifebookContainer>();
			logger.Information($"Sarting application with name: {env.ApplicationName}");
			logger.Information($"On environment: {env.EnvironmentName}");
			BeforeAppConfiguration(app);

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			Configuration = app.ApplicationServices.GetService<IConfiguration>();
			var serviceRegister = app.ApplicationServices.GetService<IServiceRegister>();
			logger.Information($"Registering application to consul.");

			app.AddHealthChecks(Configuration);

			app.Map("/service/configuration", (app) =>
			{
				app.Run(async ctx =>
				{
					ctx.Response.StatusCode = StatusCodes.Status200OK;
					await ctx.Response.WriteAsync(JObject.FromObject(Configuration.GetAll()).ToString());
				});
			});

			hostApplicationLifetime.ApplicationStarted.Register(() =>
			{
				ServiceStarted(app, container);
			});

			hostApplicationLifetime.ApplicationStopped.Register(() => {
				serviceRegister.Deregister();
				ServiceStopped(app, container);
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			AfterAppConfiguration(app, Configuration);
		}
	}
}