using System;
using System.Collections.Generic;
using lifebook.app.apploader.services.Database;
using lifebook.app.apploader.services.Models;
using Microsoft.EntityFrameworkCore;

namespace lifebook.app.apploader.services.Repository
{
    public class UserAppsRepository : AppDbContext, IRepository<UserApps>
    {
        private DbSet<UserApps> UserApps { get; }

        public bool Add(UserApps app)
        {
            throw new NotImplementedException();
        }

        public List<UserApps> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserApps GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(UserApps app)
        {
            throw new NotImplementedException();
        }
    }
}
