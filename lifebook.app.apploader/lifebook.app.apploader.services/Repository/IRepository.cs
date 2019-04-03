using System;
using System.Collections.Generic;
using lifebook.app.apploader.services.Models;

namespace lifebook.app.apploader.services.Repository
{
    public interface IRepository
    {
        bool Add(App app);
        List<App> GetAll();
        App GetById(Guid id);
        bool Update(App app);
    }
}