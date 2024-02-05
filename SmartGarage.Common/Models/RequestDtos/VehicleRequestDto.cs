using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.RequestDtos;

public class VehicleRequestDto
{
    [Required]
    public Guid BrandId { get; set; }
    [Required]
    public Guid ModelId { get; set; }
    [Required]
    [StringLength(17)]
    public string VIN { get; set; }

    [Required, Range(1886, int.MaxValue)]
    public int CreationYear { get; set; }

    [Required]
    public string LicensePlate { get; set; }
}