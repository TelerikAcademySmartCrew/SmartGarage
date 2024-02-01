using SmartGarage.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.Data.Models
{
	public class VehicleModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

		[Required]
		public int BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public VehicleBrand Brand { get; set; } = null!;

        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}