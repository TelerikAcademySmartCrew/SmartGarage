namespace SmartGarage.Models
{
    public class VisitViewModel
    {
        public DateTime DateCreated { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        public List<VisitServiceViewModel> VehicleServices { get; set; } = new List<VisitServiceViewModel>();
    }
}
