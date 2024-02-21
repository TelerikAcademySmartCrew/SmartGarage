using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.RequestDtos
{
    public class VehicleModelRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid BrandId { get; set; }
    }
}
