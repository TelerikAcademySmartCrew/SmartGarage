using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Data.Models
{
	public class VehicleBrand
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<VehicleModel> Models { get; set; } = new List<VehicleModel>();

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}