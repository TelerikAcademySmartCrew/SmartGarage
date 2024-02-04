namespace SmartGarage.Data.Models.ViewModels
{
    public class VehicleViewModel
    {
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int ProductionYear { get; set; }
        public string VIN { get; set; } = null!;
        public string LicensePlateNumber { get; set; } = null!;
    }
}
