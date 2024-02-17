using SmartGarage.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.ViewModels
{
    public class RegisterVehicleViewModel
    {
        [Required, MaxLength(40)]
        public string RegisterBrand { get; set; } = string.Empty;
        [Required, MaxLength(40)]
        public string RegisterModel { get; set; } = string.Empty;
        [Required, MaxLength(17), ValidateVIN(ErrorMessage = "VIN number must be with 17 characters long.")]
        public string VIN { get; set; } = string.Empty;
        [Required, ValidateProductionYear(ErrorMessage = "Year must be a valid number.")]
        public int CreationYear { get; set; }
        [Required]
        public string LicensePlate { get; set; } = string.Empty;
        [Required]
        public string CustomerEmail { get; set; } = string.Empty;
        public ICollection<RegisterVehicleBrandViewModel> Brands { get; set; } = new List<RegisterVehicleBrandViewModel>();
    }
}
