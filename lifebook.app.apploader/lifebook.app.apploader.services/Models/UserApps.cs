using System;
using System.Collections.Generic;

namespace lifebook.app.apploader.services.Models
{
    public class UserApps
    {
        public List<Guid> UserId { get; set; }
        public List<Guid> AppId { get; set; }
    }
}