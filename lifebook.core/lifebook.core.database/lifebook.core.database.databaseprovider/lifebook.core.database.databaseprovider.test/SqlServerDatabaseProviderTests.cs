using System;
using lifebook.core.database.databaseprovider.services;
using Xunit;

namespace lifebook.core.database.databaseprovider.test
{
    public class SqlServerDatabaseProviderTests
    {
        SqlServerDatabaseProvider sqlServer = new TestSqlDatabaseProvider();
        public SqlServerDatabaseProviderTests()
        {
        }

        [Fact]
        public void ConnectionWorks()
        {
            Assert.True(sqlServer.Database.CanConnect());
        }

        private class TestSqlDatabaseProvider : SqlServerDatabaseProvider
        {
            public TestSqlDatabaseProvider() : base("master")
            {
            }
        }
    }
}
