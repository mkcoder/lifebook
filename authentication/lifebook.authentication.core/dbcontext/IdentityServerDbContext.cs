using System;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using lifebook.core.database.databaseprovider.services;

namespace lifebook.authentication.core.dbcontext
{
    public class IdentityServerDbContext : SqlServerDatabaseProvider
    {
        public ConfigurationDbContext ConfigurationDbContext { get; private set; }
        public PersistedGrantDbContext PersistedGrantDbContext { get; private set; }
        public string ConnectionString => $@"Server={Host},{Port};Database={DatabaseName};User={Username};Password={Password};";

        public IdentityServerDbContext(ConfigurationDbContext configurationDbContext, PersistedGrantDbContext persistedGrantDbContext) : base("lifebook.oauth")

        {
            ConfigurationDbContext = configurationDbContext;
            PersistedGrantDbContext = persistedGrantDbContext;
        }
    }
}
