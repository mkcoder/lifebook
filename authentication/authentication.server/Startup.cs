using System;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
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
            container.Install();
            var sqlServerProvider = container.Resolve<SqlServerDatabaseProvider>();
            var dbConnection = sqlServerProvider.Database.GetDbConnection();

            services.AddIdentityServer()
                .AddInMemoryIdentityResources(InMemoryResources.GetInMemoryIdentityResources())
                .AddInMemoryClients(InMemoryResources.GetInmemoryClients())
                .AddInMemoryApiResources(InMemoryResources.GetInMemoryApiResources())
                .AddTestUsers(InMemoryResources.GetTestUsers())
                .AddConfigurationStore(options =>
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(dbConnection.ConnectionString, sqlBuilder => sqlBuilder.MigrationsAssembly(currentAssembly));
                    }
                )
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(dbConnection.ConnectionString,
                            sql => sql.MigrationsAssembly(currentAssembly));

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

            app.UseIdentityServer(); 
        }
    }
}
