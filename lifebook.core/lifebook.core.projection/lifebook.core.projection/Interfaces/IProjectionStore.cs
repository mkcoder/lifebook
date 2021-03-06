﻿using System;
using System.Threading.Tasks;
using lifebook.core.projection.Domain;

namespace lifebook.core.projection.Interfaces
{
    public interface IProjectionStore
    {
        public IEntitySet<TEntity> GetEntitySet<TEntity>() where TEntity : EntityProjection;
        public Task<TEntity> GetAsync<TKey, TEntity>(TKey key) where TEntity : EntityProjection;
        public Task<TEntity> GetAsync<TEntity>(Guid key) where TEntity : EntityProjection;
        public TEntity Get<TKey, TEntity>(TKey key) where TEntity : EntityProjection;
        public TEntity Store<TEntity>(TEntity value) where TEntity : EntityProjection;
    }
}