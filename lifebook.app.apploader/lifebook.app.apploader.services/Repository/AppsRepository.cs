using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.app.apploader.services.Database;
using lifebook.app.apploader.services.Models;
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

        public App GetById(Guid id) => appDbContext.Apps.Where(a => a.Id == id).Single();

        public void Add(App app)
        {
            try
            {
                app.IsValid();
                appDbContext.Apps.Add(app);
                appDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(App app)
        {
            try
            {
                app.IsValid();
                appDbContext.Apps.Update(app);
                appDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
