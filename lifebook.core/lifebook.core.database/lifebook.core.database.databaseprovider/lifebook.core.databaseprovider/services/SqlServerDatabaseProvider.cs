using System;
using lifebook.core.database.databaseprovider.interfaces;
using Microsoft.EntityFrameworkCore;

namespace lifebook.core.database.databaseprovider.services
{
    public abstract class SqlServerDatabaseProvider : AbstractSQLDatabaseProvider
    {
        public SqlServerDatabaseProvider(string database) : base(database)
        {
            var config = new ConfigurationProvider();
            Username = config["SqlServerSQLDatabaseProvider.username"];
            Password = config["SqlServerSQLDatabaseProvider.password"];
            Host = config["SqlServerSQLDatabaseProvider.host"];
            Port = config["SqlServerSQLDatabaseProvider.port"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;
            // Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;
            optionsBuilder.UseSqlServer($@"Server={Host},{Port};Database={DatabaseName};User={Username};Password={Password};");
        }
    }
}
