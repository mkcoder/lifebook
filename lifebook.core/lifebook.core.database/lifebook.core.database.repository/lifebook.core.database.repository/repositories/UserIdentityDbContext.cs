using System.Collections.Generic;
using System.Linq;
using lifebook.core.database.databaseprovider.interfaces;
using lifebook.core.database.databaseprovider.service;
using Microsoft.EntityFrameworkCore;

namespace lifebook.core.database.repository.repositories
{
    public class UserIdentityDbContext : PostgreSQLDatabaseProvider
    {
        internal DbSet<User> Users { get; set; }

        public UserIdentityDbContext() : base("identity")
        {
        }
    }
}
