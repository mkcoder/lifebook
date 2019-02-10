using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using IdentityServer4.EntityFramework.DbContexts;
using lifebook.authentication.core.dbcontext;
using lifebook.core.database.databaseprovider.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace authentication.server
{
    public class Startup
    {
        private IdentityServerDbContext identityServerDbContext;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            WindsorContainer container = new WindsorContainer();
            var currentAssembly = GetType().Assembly.GetName().Name;
            container.Install(FromAssembly.Named("lifebook.authentication.core"));
            identityServerDbContext = container.Resolve<IdentityServerDbContext>();
            services.AddIdentityServer()
                .AddInMemoryIdentityResources(InMemoryResources.GetInMemoryIdentityResources())
                .AddInMemoryClients(InMemoryResources.GetInmemoryClients())
                .AddInMemoryApiResources(InMemoryResources.GetInMemoryApiResources())
                .AddTestUsers(InMemoryResources.GetTestUsers())
                .AddConfigurationStore(options =>
                    options.ResolveDbContextOptions = (provider, builder) => {
                        builder.UseSqlServer(provider.GetRequiredService<IdentityServerDbContext>().ConnectionString,
                            sql => sql.MigrationsAssembly(currentAssembly
                        ));
                    }
                )
                .AddOperationalStore(options =>
                {
                    options.ResolveDbContextOptions = (provider, builder) =>
                    {
                        builder.UseSqlServer(provider.GetRequiredService<IdentityServerDbContext>().ConnectionString,
                            sql => sql.MigrationsAssembly(currentAssembly
                        ));
                    };

                    options.EnableTokenCleanup = true;
                });
                
            return WindsorRegistrationHelper.CreateServiceProvider(container, services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            InitializeDatabase(app);
            app.UseIdentityServer(); 
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {            
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
