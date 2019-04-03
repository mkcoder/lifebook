using System.ComponentModel.DataAnnotations;
using lifebook.core.database.databaseprovider.interfaces;

namespace lifebook.app.apploader.services.Models
{
    public sealed class App : IModel
    {
        [Key]
        [Required]
        public System.Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Icon { get; set; }
        [Required]
        public string ServiceName { get; set; }

        public void IsValid()
        {
        }
    }
}
