using System;
using System.ComponentModel.DataAnnotations;
using lifebook.core.database.databaseprovider.interfaces;

namespace lifebook.app.apploader.services.Models
{
    public class UserApps : IModel
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AppId { get; set; }

        public void IsValid()
        {
        }
    }
}