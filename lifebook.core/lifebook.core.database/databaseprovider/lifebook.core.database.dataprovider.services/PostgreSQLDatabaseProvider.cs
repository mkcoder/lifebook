using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace lifebook.core.database.databaseprovider.service
{
    public class PostgreSQLDatabaseProvider : DbContext
    {
        public PostgreSQLDatabaseProvider(string database)
        {
            var config = new ConfigurationProvider();
            DatabaseName = database;
            Username = config["PostgreSQLDatabaseProvider:username"];
            Password = config["PostgreSQLDatabaseProvider:password"];
            Host = config["PostgreSQLDatabaseProvider:host"];
            Port = config["PostgreSQLDatabaseProvider:port"];
        }

        public string DatabaseName { get; }
        public string Host { get; }
        public string Port { get; }
        public string Username { get; }
        public string Password { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password}");
        }
    }
}
