using System.ComponentModel.DataAnnotations;

namespace SmartGarage.Common.Models.ViewModels
{
    public class RegisterVehicleViewModel
    {
        [Required]
        public string VIN { get; set; } = string.Empty;
        [Required]
        public int CreationYear { get; set; }
        [Required]
        public string LicensePlate { get; set; } = string.Empty;
        [Required]
        public string CustomerEmail { get; set; } = string.Empty;
        public ICollection<RegisterVehicleBrandViewModel> Brands { get; set; } = new List<RegisterVehicleBrandViewModel>();
        
        [Required, MaxLength(15)]
        public string RegisterBrand { get; set; }
        [Required, MaxLength(15)]
        public string RegisterModel { get; set; }
    }
}
