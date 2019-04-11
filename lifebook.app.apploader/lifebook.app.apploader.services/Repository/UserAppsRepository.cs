using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.app.apploader.services.Database;
using lifebook.app.apploader.services.Models;
using Microsoft.EntityFrameworkCore;

namespace lifebook.app.apploader.services.Repository
{
    public class UserAppsRepository : IRepository<UserApps>
    {
        private readonly AppDbContext appDbContext;

        public UserAppsRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public List<UserApps> GetAll() => appDbContext.UserApps.Select(s => s).ToList();

        public UserApps GetById(Guid id) => appDbContext.UserApps.First(p => p.AppId == id);

        public List<UserApps> GetAllAppsForUser(Guid userId) => appDbContext.UserApps.Where(p => p.UserId == userId).ToList();

        public void Add(UserApps app)
        {
            app.IsValid();
            appDbContext.UserApps.Add(app);
            appDbContext.SaveChanges();
        }

        public void Update(UserApps app)
        {
            app.IsValid();
            appDbContext.UserApps.Update(app);
            appDbContext.SaveChanges();
        }
    }
}
