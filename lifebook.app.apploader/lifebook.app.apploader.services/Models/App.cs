using System;
using System.ComponentModel.DataAnnotations;
using lifebook.core.database.databaseprovider.interfaces;

namespace lifebook.app.apploader.services.Models
{
    public sealed class App : IModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Icon { get; set; }
        public string ServiceName { get; set; }

        public void IsValid()
        {
        }
    }
}
