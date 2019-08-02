using System;
using Microsoft.EntityFrameworkCore;

namespace lifebook.core.database.databaseprovider.interfaces
{
    public abstract class AbstractSQLDatabaseProvider : DbContext
    {
        protected string DatabaseName { get; set; }
        protected string Host { get; set; }
        protected string Port { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }

        public AbstractSQLDatabaseProvider(string database)
        {
            DatabaseName = database;
        }
    }
}
