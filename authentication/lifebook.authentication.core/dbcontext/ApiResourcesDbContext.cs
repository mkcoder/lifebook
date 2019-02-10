using System;
using IdentityServer4.EntityFramework.Entities;
using lifebook.core.database.databaseprovider.services;
using Microsoft.EntityFrameworkCore;

namespace lifebook.authentication.core.dbcontext
{
    public class ApiResourcesDbContext : SqlServerDatabaseProvider
    {
        public DbSet<ApiResource> ApiResources { get; set; }

        public ApiResourcesDbContext() : base("lifebook.oauth")
        {
        }

    }
}
