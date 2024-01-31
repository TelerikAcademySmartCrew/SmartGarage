using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Data.Models
{
	public class ServiceType
	{
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
