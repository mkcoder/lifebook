using lifebook.core.projection.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace lifebook.core.projection.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<TEntity> Get<TEntity>() where TEntity : class;
        EntityEntry<TEntity> GetEntityEntry<TEntity>(TEntity entity) where TEntity : class;
        int TrySaveChangesOrFail();
    }
}