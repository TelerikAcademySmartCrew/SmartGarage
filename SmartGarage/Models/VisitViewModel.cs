namespace SmartGarage.Models
{
    public class VisitViewModel
    {
        public DateTime DateCreated { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        
        public ICollection<VisitRepairActivityViewModel> RepairActivities { get; set; } = new List<VisitRepairActivityViewModel>();
    }
}
