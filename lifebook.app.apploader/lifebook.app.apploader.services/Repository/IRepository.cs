using System;
using System.Collections.Generic;
using lifebook.app.apploader.services.Models;
using lifebook.core.database.databaseprovider.interfaces;

namespace lifebook.app.apploader.services.Repository
{
    public interface IRepository<T> where T: IModel
    {
        List<T> GetAll();
        T GetById(Guid id);
        void Add(T app);
        void Update(T app);
    }
}