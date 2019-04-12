using System;
using System.Collections.Generic;
using lifebook.core.database.databaseprovider.interfaces;
using lifebook.core.database.repository.repositories;

namespace lifebook.core.database.repository.interfaces
{
    public interface IRepository<T> where T: IModel
    {
        List<T> GetAll();
        T GetByGuid(Guid guid);
        T GetById(int id);
        T Add(T user);
        T Update(Guid guid, T user);
    }
}
