using System;
using System.Collections.Generic;
using lifebook.core.database.databaseprovider.interfaces;

namespace lifebook.app.apploader.services.Models
{
    public class UserApps : IModel
    {
        public List<Guid> UserId { get; set; }
        public List<Guid> AppId { get; set; }

        public void IsValid()
        {
        }
    }
}