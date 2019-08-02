using System;
using System.ComponentModel.DataAnnotations.Schema;
using lifebook.core.database.databaseprovider.interfaces;

namespace lifebook.core.database.repository.repositories
{
    [Table("users", Schema = "public")]
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
