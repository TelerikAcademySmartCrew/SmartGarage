namespace SmartGarage.Utilities.Models
{
    public class VisitViewModel
    {
        public DateTime DateCreated { get; set; }
        
        public string VehicleBrand { get; set; }
        
        public string VehicleModel { get; set; }

        public double TotalPrice { get; set; }
        
        public IEnumerable<VisitRepairActivityViewModel> RepairActivities { get; set; } = new List<VisitRepairActivityViewModel>();
    }
}
