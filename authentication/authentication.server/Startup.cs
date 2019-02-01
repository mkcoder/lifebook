using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.MsDependencyInjection;
using IdentityServer4.EntityFramework.DbContexts;
using lifebook.core.database.databaseprovider.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace authentication.server
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            WindsorContainer container = new WindsorContainer();
            var currentAssembly = GetType().Assembly.GetName().Name;
            container.Install(FromAssembly.InThisApplication(GetType().Assembly));
            container.Register(Component.For<IdentityServerDbContext>());
            var sqlServerProvider = new IdentityServerDbContext();
            var dbConnection = sqlServerProvider.Database.GetDbConnection();
            Console.WriteLine($"SQL SERVER CONNECT: {sqlServerProvider.Database.CanConnect()}");
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

    public class IdentityServerDbContext : SqlServerDatabaseProvider
    {
        public IdentityServerDbContext() : base("lifebook")
        {
        }

        public string ConnectionString => $@"Server={Host},{Port};Database={DatabaseName};User={Username};Password={Password};";
    }
}
