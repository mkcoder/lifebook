using System;
using System.Threading.Tasks;
using lifebook.core.database.databaseprovider.service;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace lifebook.core.database.databaseprovider.test
{
    public class PostgreSQLDatabaseProviderTestExplict
    {
        IdentityContext _sut = new IdentityContext("identity");

        public void BasicExample()
        {
            Assert.Contains("PostgreSQL", _sut.Database.ProviderName);
        }

        public async Task IdentityExample()
        {
            var result = await _sut.Users.ToListAsync();
            Assert.NotNull(result);
        }
    }

    public class IdentityContext : PostgreSQLDatabaseProvider
    {
        public DbSet<Users> Users { get; set; }
        public IdentityContext(string database) : base(database)
        {
        }
    }

    public class Users
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] Claims { get; set; }
    }
}