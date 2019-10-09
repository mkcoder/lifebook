using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using lifebook.core.services.interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace lifebook.core.projection.Services
{
    public sealed class DatabaseProjectionStore : DbContext, IProjectionStore
    {
        private readonly IConfiguration _configuration;

        public DatabaseProjectionStore(DbContextOptions<DatabaseProjectionStore> options, IConfiguration configuration)
            : base(options)
        {
            Database.EnsureCreated();
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_configuration.GetValue("ProjectionConnectionString"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => typeof(EntityProjection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();
            foreach (var type in assemblies)
            {
                modelBuilder
                    .Entity(type, bt =>
                        {
                            var props = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
                            foreach (var prop in props)
                            {
                                bt.Ignore(prop.Name);
                            }
                            bt.HasKey(new string[] { "AggregateId" });
                            bt.Property<string>("JSON");
                        }
                    );
            }

            base.OnModelCreating(modelBuilder);
        }

        public IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : EntityProjection
        {
            return new DbEntitySet<TEntity>(Set<TEntity>());
        }

        public async Task<TEntity> Get<TKey, TEntity>(TKey key) where TEntity : EntityProjection
        {
            return await FindAsync<TEntity>(new object[] { key });
        }

        public async Task<TEntity> Get<TEntity>(Guid key) where TEntity : EntityProjection
        {
            return await Get<Guid, TEntity>(key);
        }

        public TEntity Store<TEntity>(TEntity value) where TEntity : EntityProjection
        {
            var dbset = this.Set<TEntity>();//this.Find(typeof(), new object[] { });
            var entry = Entry(value);
            entry.Property("JSON").CurrentValue = JObject.FromObject(value).ToString();
            dbset.Add(value);
            SaveChanges();
            return value;
        }
    }
}