using System;
using IdentityServer4.EntityFramework.Entities;
using lifebook.core.database.databaseprovider.services;
using Microsoft.EntityFrameworkCore;

namespace lifebook.authentication.core.dbcontext
{
    public class ClientDbContext : SqlServerDatabaseProvider
    {
        public DbSet<Client> Clients { get; set; }

        public ClientDbContext() : base("lifebook.oauth")
        {
        }
    }
}
