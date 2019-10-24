using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using lifebook.core.logging.interfaces;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services.StreamTracker;
using lifebook.core.services.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace lifebook.core.projection.Services.ProjectionStore
{
    public class PostgresContextCreator : DbContext, IApplicationContextCreator
    {
        private readonly DbContextOptions<PostgresContextCreator> _options;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private List<Type> assemblies;
        private readonly RelationalDatabaseCreator _rdbc;
        private readonly string TableNamePrefix;

        public PostgresContextCreator(DbContextOptions<PostgresContextCreator> options, IConfiguration configuration,
                                      ILogger logger)
        {
            _options = options;
            _configuration = configuration;
            _logger = logger;
            _rdbc = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
            TableNamePrefix = $"{_configuration.GetValue("ServiceName")}";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetValue("ProjectionConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<StreamTrackingInformation>();
        }

        public void CreateContext()
        {
            _logger.Information($"Started Building Context for project: {GetType().FullName}.");
            assemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                        .Where(x => typeof(EntityProjection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                        .ToList();

            if (!_rdbc.Exists())
            {
                _rdbc.Create();
            }

            try
            {
                CreateStreamTrackingInformation();
                CreateTablesIfNotExist();
            }
            catch (Exception ex)
            {
                _logger.Information($"Failed attempting to create tables {ex.Message}");
            }
        }

        private void CreateStreamTrackingInformation()
        {
            var conn = Database.GetDbConnection();
            if (conn.State.Equals(ConnectionState.Closed)) conn.Open();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $@"
CREATE TABLE public.""StreamTrackingInformation""
(
    ""Id"" uuid NOT NULL,
    ""StreamKey"" text COLLATE pg_catalog.""default"",
    ""StreamId"" text COLLATE pg_catalog.""default"",
    ""LastEventRead"" bigint NOT NULL,
    CONSTRAINT ""PK_StreamTrackingInformation"" PRIMARY KEY(""Id"")
)";
                _logger.Information($"Running SQL Query: {command.CommandText}");
                var result = command.ExecuteNonQuery();
                _logger.Information($"Result From SQL Query: {result}");
            }
        }

        private void CreateTablesIfNotExist()
        {
            foreach (var type in assemblies)
            {
                if (type.Name.ToLower() == "entityprojection") continue;
                _logger.Information("Running SQL Query To Create Table If Not Exist");
                _logger.Information($"Type: {type}");
                var result = CreateTableIfNotExists(type.Name);
            }
        }

        private bool CreateTableIfNotExists(string name)
        {
            object result = null;
            var conn = Database.GetDbConnection();
            if (conn.State.Equals(ConnectionState.Closed)) conn.Open();
            using (var command = conn.CreateCommand())
            {
                var tableName = $"{TableNamePrefix}_{name}";
                command.CommandText = $@"
CREATE TABLE IF NOT EXISTS {tableName} (
    ""Key"" uuid NOT NULL,
    ""LastUpdated"" timestamp without time zone NOT NULL,
    ""JSON"" jsonb,
    CONSTRAINT ""PK_${tableName}"" PRIMARY KEY(""Key"")
);";
                _logger.Information($"Running SQL Query: {command.CommandText}");
                result = command.ExecuteNonQuery();
                _logger.Information($"Result From SQL Query: {(result == null ? "null result" : result)}");
            }
            return result != null;
        }
    }
}
