using System;
using System.Collections.Generic;
using System.Linq;
using lifebook.core.database.repository.interfaces;

namespace lifebook.core.database.repository.repositories
{
    public class UserIdentityRepository : IRepository<User>
    {
        private readonly UserIdentityDbContext _userIdentityDbContext;

        public UserIdentityRepository(UserIdentityDbContext userIdentityDbContext)
        {
            _userIdentityDbContext = userIdentityDbContext;
        }

        public UserIdentityRepository()
        {
        }

        // GETS 
        public List<User> GetAllUsers() => _userIdentityDbContext.Users.ToList();
        public User GetUserById(int id) => _userIdentityDbContext.Users.FirstOrDefault(u => u.Id == id);
        public User GetUserByGuid(Guid guid) => _userIdentityDbContext.Users.FirstOrDefault(u => u.UserId == guid);

        // SETS
        public User AddNewUser(User user)
        {
            user.IsValid();
            _userIdentityDbContext.Users.Add(user);
            _userIdentityDbContext.SaveChanges();
            return user;
        }
    }
}
