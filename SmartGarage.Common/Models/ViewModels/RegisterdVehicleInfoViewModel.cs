namespace SmartGarage.Common.Models.ViewModels
{
    public class RegisterdVehicleInfoViewModel
    {
        public string RegisterBrand { get; set; } = string.Empty;
        public string RegisterModel { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public int CreationYear { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;        
    }
}
