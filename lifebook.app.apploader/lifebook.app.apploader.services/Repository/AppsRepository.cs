using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.app.apploader.services.Database;
using lifebook.app.apploader.services.Models;
using Microsoft.EntityFrameworkCore;

namespace lifebook.app.apploader.services.Repository
{
    public class AppsRepository : AppDbContext
    {
        private DbSet<App> Apps { get; }

        public List<App> GetAll() => Apps.ToList();

        public App GetById(Guid id) => Apps.Where(a => a.Id == id).Single();

        public bool Add(App app)
        {
            try
            {
                app.IsValid();
                Apps.Add(app);
                SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(App app)
        {
            try
            {
                app.IsValid();
                Apps.Update(app);
                SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
