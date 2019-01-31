using System;
using lifebook.core.database.databaseprovider.interfaces;
using Microsoft.EntityFrameworkCore;

namespace lifebook.core.database.databaseprovider.services
{
    public abstract class SqlServerDatabaseProvider : AbstractSQLDatabaseProvider
    {
        public SqlServerDatabaseProvider(string database) : base(database)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Data Source=190.190.200.100,1433;Network Library=DBMSSOCN;
            // Initial Catalog = myDataBase; User ID = myUsername; Password = myPassword;
            optionsBuilder.UseSqlServer($@"Data Source={Host},{Port};Database={DatabaseName};User ID = {Username}; Password = {Password};Trusted_Connection=True;Network Library=DBMSSOCN;");
        }
    }
}
