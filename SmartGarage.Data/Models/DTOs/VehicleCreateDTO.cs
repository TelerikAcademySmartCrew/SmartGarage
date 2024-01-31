using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Data.Models.DTOs
{
    public class VehicleCreateDTO
    {
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public string VIN { get; set; }

        [Required, Range(1886, int.MaxValue)]
        public int CreationYear { get; set; }

        [Required]
        public string LicensePlate { get; set; }
    }
}
