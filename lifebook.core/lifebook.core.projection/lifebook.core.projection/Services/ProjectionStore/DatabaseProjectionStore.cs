using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace lifebook.core.projection.Services
{
    public class DatabaseProjectionStore : IProjectionStore
    {
        private readonly IApplicationContext  _applicationContext;
        private static readonly object _lock = new object();

        public DatabaseProjectionStore(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : EntityProjection
        {
            return new DbEntitySet<TEntity>(_applicationContext.Get<TEntity>());
        }

        public async Task<TEntity> GetAsync<TKey, TEntity>(TKey key) where TEntity : EntityProjection
        {
            var dbset = _applicationContext.Get<TEntity>();
            var result = await dbset.FindAsync(new object[] { key });
            //var mr = EF.Property<TEntity>(result, "JSON");
            return result;
        }

        public async Task<TEntity> GetAsync<TEntity>(Guid key) where TEntity : EntityProjection
        {
            var dbset = _applicationContext.Get<TEntity>();
            var result = dbset.Select(p => EF.Property<TEntity>(p, "JSON")).ToList().FirstOrDefault(p => p.Key == key);
            return result;
        }

        public TEntity Get<TKey, TEntity>(TKey key) where TEntity : EntityProjection
        {
            return _applicationContext.Get<TEntity>().Find(new object[] { key });
        }

        public TEntity Store<TEntity>(TEntity value) where TEntity : EntityProjection
        {
            lock (_lock)
            {
                var dbset = _applicationContext.Get<TEntity>();//this.Find(typeof(), new object[] { });
                var entry = _applicationContext.GetEntityEntry(value);
                entry.Entity.LastUpdated = DateTime.UtcNow;
                entry.Property("JSON").CurrentValue = value;
                if (dbset.Any(e => e.Key == value.Key))
                {
                    dbset.Update(value);
                }
                else
                {
                    dbset.Add(value);
                }
                _applicationContext.TrySaveChangesOrFail();
            }
            return value;
        }
    }
}