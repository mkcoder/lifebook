using System;
using System.Collections.Generic;
using lifebook.core.database.databaseprovider.interfaces;
using lifebook.core.database.repository.repositories;

namespace lifebook.core.database.repository.interfaces
{
    public interface IRepository<T> where T: IModel
    {
        List<User> GetAllUsers();
        T GetUserByGuid(Guid guid);
        T GetUserById(int id);
        T AddNewUser(T user);
    }
}
