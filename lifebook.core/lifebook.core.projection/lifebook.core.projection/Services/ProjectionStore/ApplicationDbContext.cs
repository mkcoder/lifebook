using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Castle.Windsor;
using lifebook.core.logging.interfaces;
using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.projection.Services.StreamTracker;
using lifebook.core.services.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace lifebook.core.projection.Services
{
    public class ApplicationDbContext : DbContext, IApplicationContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly string TableNamePrefix;
        private static readonly object _lock = new object();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration, ILogger logger)
            : base(options)
        {
            _configuration = configuration;
            _logger = logger;
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

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                        .Where(x => typeof(EntityProjection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                        .ToList();

            foreach (var type in assemblies)
            {
                if (type.Name.ToLower() == "entityprojection") continue;
                modelBuilder
                    .Entity(type, bt =>
                    {
                        var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        foreach (var prop in props)
                        {
                            bt.Ignore(prop.Name);
                        }
                        bt.ToTable($"{TableNamePrefix}_{type.Name}".ToLower());
                        bt.HasKey(new string[] { "Key" });
                        bt.Property<string>("JSON").HasColumnType("jsonb");
                    }
                );
            }

            modelBuilder.Entity<StreamTrackingInformation>();
        }

        public DbSet<TEntity> Get<TEntity>() where TEntity : class
        {
            DbSet<TEntity> result = null;
            lock (_lock)
            {
                result = Set<TEntity>();
            }
            return result;
        }

        public int TrySaveChangesOrFail()
        {
            lock (_lock)
            {
                try
                {
                    return SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Something went wrong");
                    throw ex;
                }
            }
        }

        public EntityEntry<TEntity> GetEntityEntry<TEntity>(TEntity entity) where TEntity : class
        {
            EntityEntry<TEntity> result = null;
            lock (_lock)
            {
                result = Entry(entity);
            }
            return result;
        }
    }
}