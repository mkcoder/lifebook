using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.app.apploader.services.Database;
using lifebook.app.apploader.services.Models;
using lifebook.core.database.repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace lifebook.app.apploader.services.Repository
{
    public class AppsRepository : IRepository<App>
    {
        private readonly AppDbContext appDbContext;

        public AppsRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public List<App> GetAll() => appDbContext.Apps.ToList();

        public App GetByGuid(Guid id) => appDbContext.Apps.Where(a => a.Id == id).Single();

        public App Add(App app)
        {
            try
            {
                app.IsValid();
                appDbContext.Apps.Add(app);
                appDbContext.SaveChanges();
                return app;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public App Update(Guid id, App app)
        {
            try
            {
                app.IsValid();
                appDbContext.Apps.Update(app);
                appDbContext.SaveChanges();
                return app;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public App GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
