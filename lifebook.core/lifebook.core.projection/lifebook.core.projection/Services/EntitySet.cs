using lifebook.core.projection.Domain;
using lifebook.core.projection.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace lifebook.core.projection.Services
{
    public class DbEntitySet<T> : IEntitySet<T> where T : EntityProjection
    {
        private DbSet<T> _entity;

        public DbEntitySet(DbSet<T> entity)
        {
            _entity = entity;
        }

        public List<T> ToList()
        {
            return _entity.Select(p => JObject.Parse(EF.Property<string>(p, "JSON")).ToObject<T>()).ToList();
        }
    }
}