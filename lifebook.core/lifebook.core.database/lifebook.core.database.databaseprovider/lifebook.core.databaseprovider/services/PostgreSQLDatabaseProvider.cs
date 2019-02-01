using System;
using System.Data.Common;
using JetBrains.Annotations;
using lifebook.core.database.databaseprovider.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace lifebook.core.database.databaseprovider.service
{
    public class PostgreSQLDatabaseProvider : AbstractSQLDatabaseProvider
    {
        public PostgreSQLDatabaseProvider(string database) : base(database)
        {
            var config = new ConfigurationProvider();
            Username = config["PostgreSQLDatabaseProvider.username"];
            Password = config["PostgreSQLDatabaseProvider.password"];
            Host = config["PostgreSQLDatabaseProvider.host"];
            Port = config["PostgreSQLDatabaseProvider.port"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password}");
        }

    }
}
