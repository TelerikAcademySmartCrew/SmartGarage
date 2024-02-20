using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.RequestDtos
{
    public class VehicleBrandRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
