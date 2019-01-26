using System;
using lifebook.core.database.databaseprovider.interfaces;

namespace lifebook.core.database.repository.repositories
{
    public class User : IModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] Claims { get; set; }

        public void IsValid()
        {
        }
    }
}
