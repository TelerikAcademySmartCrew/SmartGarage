using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartGarage.Data.Models
{
	public class VehicleModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public VehicleBrand Brand { get; set; }
    }
}