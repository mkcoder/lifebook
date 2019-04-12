using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.app.apploader.services.Database;
using lifebook.app.apploader.services.Models;
using lifebook.core.database.repository.interfaces;
using lifebook.core.database.repository.repositories;
using lifebook.core.logging.interfaces;
using Microsoft.EntityFrameworkCore;

namespace lifebook.app.apploader.services.Repository
{
    public class UserAppsRepository : IRepository<UserApps>
    {
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public UserAppsRepository(AppDbContext appDbContext, ILogger logger)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
        }

        public List<UserApps> GetAll() => appDbContext.UserApps.Select(s => s).ToList();

        public UserApps GetByGuid(Guid guid) => appDbContext.UserApps.First(p => p.AppId == guid);

        public UserApps GetById(int id) => appDbContext.UserApps.First(p => p.Id == id);

        public List<UserApps> GetAllAppsForUser(Guid userId) => appDbContext.UserApps.Where(p => p.UserId == userId).ToList();

        public UserApps Add(UserApps app)
        {
            app.IsValid();
            appDbContext.UserApps.Add(app);
            appDbContext.SaveChanges();
            return app;
        }

        public UserApps Update(Guid guid, UserApps app)
        {
            app.IsValid();
            appDbContext.UserApps.Update(app);
            appDbContext.SaveChanges();
            return app;
        }
    }
}
